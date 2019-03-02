using System;
using System.IO;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Metadata;
using CustomScaffold.Templates;
using System.Linq;
using Database.Models;
using Microsoft.EntityFrameworkCore;

using Jaster.Json;

namespace CustomScaffold
{
    public class EntityFile
    {
        public string path;
        public string data;
    }

    public interface IEntityFileGenerator
    {
        EntityFile Generate(IEntityType entity);
    }

    class Program
    {
        static List<IEntityFileGenerator> _generators;

        static void Main(string[] args)
        {
            _generators = new List<IEntityFileGenerator>();
            _generators.Add(new IndexGenerator());
            _generators.Add(new CreateEditGenerator("Create"));
            _generators.Add(new CreateEditGenerator("Edit"));
            _generators.Add(new DetailsDeleteGenerator("Details"));
            _generators.Add(new DetailsDeleteGenerator("Delete"));

            Environment.CurrentDirectory = Path.GetRelativePath(Environment.CurrentDirectory, "../");

            var json = Json.Parse(File.ReadAllText("appsettings.json"));
            using (var db = new LivestockContext(json.Get("ConnectionStrings").Get("Livestock").As<string>()))
            {
                _generators.Add(new ControllerGenerator(db));
                foreach (var entity in db.Model.GetEntityTypes())
                {
                    foreach(var gen in _generators)
                    {
                        var info = gen.Generate(entity);
                        if(!Directory.Exists(Path.GetDirectoryName(info.path)))
                            Directory.CreateDirectory(Path.GetDirectoryName(info.path));

                        File.WriteAllText(info.path, info.data);
                    }
                }
            }
        }
    }

    public static class Extensions
    {
        public static IProperty GetDisplayNameField(this IEntityFileGenerator _, IEntityType entity)
        {
            IProperty prop = null;
            
            foreach(var evilProp in entity.GetProperties())
            {
                if(evilProp.Name == "Name"
                || evilProp.Name == "Description"
                || evilProp.Name == "MobileNumber"
                || evilProp.Name == "RegistrationNumber"
                || evilProp.Name == "VehicleTrailerMapId")
                {
                    prop = evilProp;
                    break;
                }
            }

            if(prop == null)
            {
                throw new Exception($"{String.Join('\n', entity.GetProperties().Select(p => p.Name))}");
            }

            return prop;
        }

        public static IEnumerable<IProperty> GetPropertiesFilterInternal(this IEntityType entity)
        {
            return entity.GetProperties()
                         .Where(p => !p.IsInternalFor(entity));
        }

        public static bool IsInternalFor(this IProperty property, IEntityType entity)
        {
            return (entity.GetKeys().First().Properties.Contains(property)) // Ignore the primary key
                || (property.Name == "Timestamp")
                || (property.Name == "VersionNumber");
        }

        public static IEnumerable<IProperty> OrderByUserExperience(this IEnumerable<IProperty> props, IEntityType entity)
        {
            // Primary key -> Name -> Description -> Foreign keys -> Other fields -> Comment
            return props.OrderBy(p => entity.GetKeys().First().Properties.First().Name == p.Name ? 1 : 2)
                        .ThenBy (p => p.Name == "Name" ? 1 : 2)
                        .ThenBy (p => p.Name == "Description" ? 1 : 2)
                        .ThenBy (p => entity.GetForeignKeys().Any(fk => fk.Properties.First().Name == p.Name) ? 1 : 2)
                        .ThenBy (p => p.Name)
                        .ThenBy (p => p.Name != "Comment" ? 1 : 2);
        }
    }

    class ControllerGenerator : IEntityFileGenerator
    {
        readonly LivestockContext db;
        public ControllerGenerator(LivestockContext db)
        {
            this.db = db;
        }

        public EntityFile Generate(IEntityType entity)
        {
            var template = new Controller();
            template.EntityName = entity.ClrType.Name;
            template.EntityIdName = entity.GetKeys().First().Properties.First().Name;

            // Critter.Include(c => c.CritterType).Include(c => c.DadCritter).Include(c => c.MumCritter)
            template.ContextGetterString = template.EntityName;
            var includes = String.Join('.', entity.GetForeignKeys()
                                                  .Select(fk => $"Include(v => v.{fk.DependentToPrincipal.Name})")
                                      );
            if(!String.IsNullOrWhiteSpace(includes))
                template.ContextGetterString += "."+includes;

            //ViewData["CritterTypeId"] = new SelectList(_context.CritterType, "CritterTypeId", "Comment");
            template.ForeignKeyDropDownCreationString = String.Join('\n', this.CreateFKDropDown(entity).Select(l => l + ");"));

            //ViewData["CritterTypeId"] = new SelectList(_context.CritterType, "CritterTypeId", "Comment", critter.CritterTypeId);
            if(entity.GetForeignKeys().Count() > 0)
            {
                template.ForeignKeyDropDownCreationWithSelectedIndexString
                    = this.CreateFKDropDown(entity)
                          .Zip(entity.GetForeignKeys()
                               .Select(fk => fk.Properties.First().Name), 
                               (str, name) => str + $", val.{name}" + ");"
                              )
                          .Aggregate((one, two) => one + "\n" + two);
            }
            else
                template.ForeignKeyDropDownCreationWithSelectedIndexString = "";

            // "CritterTypeID,Description,Comment,Version, [etc.]"
            // SELF NOTE: All 'internal' fields should have a hidden input generated for them.
            template.FormBindingParams
                = entity.GetProperties()
                        .Select(p => p.Name)
                        .Aggregate((one, two) => one + "," + two);

            // HACK: Special support for Gender, since it has a max length of 1
            // if(String.IsNullOrWhitespace(val.Comment))
            //     val.Comment = "N/A";
            template.FixNullFieldsCode
                = entity.GetProperties()
                        .Where(p => p.ClrType.UnderlyingSystemType == typeof(string))
                        .Select(p => $"if(String.IsNullOrWhiteSpace(val.{p.Name})) val.{p.Name} = \"{(p.Name == "Gender" ? "?" : "N/A")}\";")
                        .Aggregate((one, two) => one + "\n" + two);

            // Create the authorise attribute
            // [AimAuthorize(RolesOR = "admin,staff")]
            var rolesOR 
                = String.Join(',',
                              db.MenuItem.Include(i => i.MenuHeader)
                                .Where(i => i.MenuHeader.ApplicationCode == 1 && i.Controller == entity.ClrType.Name)
                                .Select(i => i.MenuHeader.Role.Description)
                                .Distinct()
                             );

            if (String.IsNullOrWhiteSpace(rolesOR))
                rolesOR = "[Forbidden to all]";

            template.ControllerAuthAttrib = $"[AimAuthorize(RolesOR: \"{rolesOR}\")]";

            return new EntityFile(){ data = template.TransformText(), path = Path.Join("Controllers/Generated/", entity.ClrType.Name + ".cs") };
        }

        List<string> CreateFKDropDown(IEntityType entity)
        {
            return 
                entity.GetForeignKeys()
                .Select(fk => $"ViewData[\"{fk.Properties.First().Name}\"] = new SelectList(" 
                                + $"_context.{fk.PrincipalEntityType.ClrType.Name}, "
                                + $"\"{fk.PrincipalKey.Properties.First().Name}\", " 
                                + $"\"{this.GetDisplayNameField(fk.PrincipalEntityType).Name}\"")
                .ToList();
        }
    }

    class IndexGenerator : IEntityFileGenerator
    {
        public EntityFile Generate(IEntityType entity)
        {
            var template = new Index();
            template.EntityName = entity.ClrType.Name;
            template.EntityIdName = entity.GetKeys().First().Properties.First().Name;
            template.ColumnNames = entity.GetPropertiesFilterInternal().OrderByUserExperience(entity).Select(p => p.Name).ToList();

            template.DisplayOverrides = new Dictionary<string, string>();
            foreach(var fk in entity.GetForeignKeys())
            {
                template.DisplayOverrides[fk.Properties.First().Name]
                    = $"{fk.DependentToPrincipal.Name}.{this.GetDisplayNameField(fk.PrincipalEntityType).Name}";
            }

            return new EntityFile() { data = template.TransformText(), path = $"Views/Generated/{entity.ClrType.Name}/Index.cshtml" };
        }
    }

    class CreateEditGenerator : IEntityFileGenerator
    {
        public string Action;

        public CreateEditGenerator(string action)
        {
            this.Action = action;
        }

        public EntityFile Generate(IEntityType entity)
        {
            var template = new CreateEdit();
            template.Action = this.Action;
            template.EntityName = entity.ClrType.Name;
            template.EntityIdName = entity.GetKeys().First().Properties.First().Name;
            template.ColumnNames = entity.GetProperties()
                                         .OrderByUserExperience(entity)
                                         .Select(p => 
                                                {
                                                    if(entity.GetForeignKeys().Select(k => k.Properties[0].Name).Contains(p.Name))
                                                        return p.Name + CreateEdit.IDENTIFIER_FK;
                                                    else if(p.IsInternalFor(entity))
                                                        return p.Name + CreateEdit.IDENTIFIER_HIDDEN;
                                                    else if(p.ClrType == typeof(DateTime))
                                                        return p.Name + CreateEdit.IDENTIFIER_DATETIME;
                                                    else
                                                        return p.Name;
                                                })
                                         .ToList();

            return new EntityFile() { data = template.TransformText(), path = $"Views/Generated/{entity.ClrType.Name}/{this.Action}.cshtml" };
        }
    }

    class DetailsDeleteGenerator : IEntityFileGenerator
    {
        public string Action;

        public DetailsDeleteGenerator(string action)
        {
            this.Action = action;
        }

        public EntityFile Generate(IEntityType entity)
        {
            var template = new DetailsDelete();
            template.Action = this.Action;
            template.EntityName = entity.ClrType.Name;
            template.EntityIdName = entity.GetKeys().First().Properties.First().Name;
            template.ColumnNames = entity.GetPropertiesFilterInternal().OrderByUserExperience(entity).Select(p => p.Name).ToList();

            return new EntityFile() { data = template.TransformText(), path = $"Views/Generated/{entity.ClrType.Name}/{this.Action}.cshtml" };
        }
    }
}
