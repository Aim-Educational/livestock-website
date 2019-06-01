using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Database.Migrations
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "admm_group_map",
                columns: table => new
                {
                    admm_group_map_id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    group_entity_user_id = table.Column<int>(nullable: false),
                    group_entity_data_id = table.Column<int>(nullable: false),
                    group_entity_data_type = table.Column<int>(nullable: false),
                    timestamp = table.Column<byte[]>(rowVersion: true, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_admm_group_map", x => x.admm_group_map_id);
                });

            migrationBuilder.CreateTable(
                name: "admu_group",
                columns: table => new
                {
                    admu_group_id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    name = table.Column<string>(maxLength: 100, nullable: false),
                    description = table.Column<string>(maxLength: 255, nullable: false),
                    group_type = table.Column<int>(nullable: false),
                    timestamp = table.Column<byte[]>(rowVersion: true, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_admu_group", x => x.admu_group_id);
                });

            migrationBuilder.CreateTable(
                name: "al_user_info",
                columns: table => new
                {
                    al_user_info_id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    first_name = table.Column<string>(maxLength: 150, nullable: false),
                    last_name = table.Column<string>(maxLength: 150, nullable: false),
                    tos_consent = table.Column<bool>(nullable: false),
                    privacy_consent = table.Column<bool>(nullable: false),
                    timestamp = table.Column<byte[]>(rowVersion: true, nullable: false),
                    comment = table.Column<string>(maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_al_user_info", x => x.al_user_info_id);
                });

            migrationBuilder.CreateTable(
                name: "contact",
                columns: table => new
                {
                    contact_id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    name = table.Column<string>(maxLength: 50, nullable: false),
                    address = table.Column<string>(maxLength: 100, nullable: false),
                    is_customer = table.Column<bool>(nullable: false),
                    is_supplier = table.Column<bool>(nullable: false),
                    phone_number1 = table.Column<string>(maxLength: 50, nullable: false),
                    phone_number2 = table.Column<string>(maxLength: 50, nullable: false),
                    phone_number3 = table.Column<string>(maxLength: 50, nullable: false),
                    phone_number4 = table.Column<string>(maxLength: 50, nullable: false),
                    email_address = table.Column<string>(maxLength: 100, nullable: false),
                    comment = table.Column<string>(maxLength: 50, nullable: false),
                    timestamp = table.Column<byte[]>(rowVersion: true, nullable: false),
                    version_number = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_contact", x => x.contact_id);
                });

            migrationBuilder.CreateTable(
                name: "critter_image",
                columns: table => new
                {
                    critter_image_id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    data = table.Column<byte[]>(nullable: false),
                    timestamp = table.Column<byte[]>(rowVersion: true, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_critter_image", x => x.critter_image_id);
                });

            migrationBuilder.CreateTable(
                name: "critter_life_event_datetime",
                columns: table => new
                {
                    critter_life_event_give_birth_id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    date_time = table.Column<DateTime>(type: "datetime", nullable: false),
                    comment = table.Column<string>(maxLength: 50, nullable: false),
                    timestamp = table.Column<byte[]>(rowVersion: true, nullable: false),
                    version_number = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_critter_life_event_give_birth", x => x.critter_life_event_give_birth_id);
                });

            migrationBuilder.CreateTable(
                name: "critter_type",
                columns: table => new
                {
                    critter_type_id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    name = table.Column<string>(maxLength: 50, nullable: false),
                    gestration_period = table.Column<int>(nullable: false),
                    comment = table.Column<string>(maxLength: 50, nullable: false),
                    timestamp = table.Column<byte[]>(rowVersion: true, nullable: false),
                    version_number = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_critter_type", x => x.critter_type_id);
                });

            migrationBuilder.CreateTable(
                name: "enum_critter_life_event_category",
                columns: table => new
                {
                    enum_critter_life_event_category_id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    description = table.Column<string>(maxLength: 50, nullable: false),
                    timestamp = table.Column<byte[]>(rowVersion: true, nullable: false),
                    version_number = table.Column<int>(nullable: false),
                    comment = table.Column<string>(maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_enum_critter_life_event_category", x => x.enum_critter_life_event_category_id);
                });

            migrationBuilder.CreateTable(
                name: "enum_critter_life_event_type",
                columns: table => new
                {
                    enum_critter_life_event_type_id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    description = table.Column<string>(maxLength: 50, nullable: false),
                    enum_critter_life_event_category_id = table.Column<int>(nullable: false),
                    data_type = table.Column<string>(maxLength: 50, nullable: false),
                    comment = table.Column<string>(maxLength: 50, nullable: false),
                    timestamp = table.Column<byte[]>(rowVersion: true, nullable: false),
                    version_number = table.Column<int>(nullable: false),
                    allow_multiple = table.Column<bool>(nullable: false),
                    flag_cant_reproduce = table.Column<bool>(nullable: false),
                    flag_end_of_system = table.Column<bool>(nullable: false),
                    flag_illness = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_enum_critter_life_event_type", x => x.enum_critter_life_event_type_id);
                });

            migrationBuilder.CreateTable(
                name: "enum_location_type",
                columns: table => new
                {
                    enum_location_type_id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    description = table.Column<string>(maxLength: 50, nullable: false),
                    comment = table.Column<string>(maxLength: 50, nullable: false),
                    timestamp = table.Column<byte[]>(rowVersion: true, nullable: false),
                    version_number = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_enum_location_type", x => x.enum_location_type_id);
                });

            migrationBuilder.CreateTable(
                name: "enum_product_type",
                columns: table => new
                {
                    enum_product_type_id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    description = table.Column<string>(maxLength: 50, nullable: false),
                    comment = table.Column<string>(maxLength: 50, nullable: false),
                    timestamp = table.Column<byte[]>(rowVersion: true, nullable: false),
                    version_number = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_enum_product_type", x => x.enum_product_type_id);
                });

            migrationBuilder.CreateTable(
                name: "enum_vehicle_life_event_type",
                columns: table => new
                {
                    enum_vehicle_life_event_type_id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    description = table.Column<string>(maxLength: 50, nullable: false),
                    comment = table.Column<string>(maxLength: 50, nullable: false),
                    timestamp = table.Column<byte[]>(rowVersion: true, nullable: false),
                    version_number = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_enum_vehicle_life_event_type", x => x.enum_vehicle_life_event_type_id);
                });

            migrationBuilder.CreateTable(
                name: "menu_item",
                columns: table => new
                {
                    menu_item_id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    title = table.Column<string>(maxLength: 50, nullable: false),
                    icon_uri = table.Column<string>(maxLength: 255, nullable: false),
                    role_id = table.Column<int>(nullable: false),
                    sequence_number = table.Column<int>(nullable: false),
                    controller = table.Column<string>(maxLength: 50, nullable: false),
                    action = table.Column<string>(maxLength: 50, nullable: false),
                    comment = table.Column<string>(maxLength: 50, nullable: false),
                    timestamp = table.Column<byte[]>(rowVersion: true, nullable: false),
                    version_number = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_menu_item", x => x.menu_item_id);
                });

            migrationBuilder.CreateTable(
                name: "role",
                columns: table => new
                {
                    role_id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    description = table.Column<string>(maxLength: 50, nullable: false),
                    comment = table.Column<string>(maxLength: 50, nullable: false),
                    timestamp = table.Column<byte[]>(rowVersion: true, nullable: false),
                    version_number = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_role", x => x.role_id);
                });

            migrationBuilder.CreateTable(
                name: "vehicle",
                columns: table => new
                {
                    vehicle_id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    name = table.Column<string>(maxLength: 50, nullable: false),
                    registration_number = table.Column<string>(maxLength: 50, nullable: false),
                    weight_kg = table.Column<double>(nullable: false),
                    comment = table.Column<string>(maxLength: 50, nullable: false),
                    timestamp = table.Column<byte[]>(rowVersion: true, nullable: false),
                    version_number = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_vehicle", x => x.vehicle_id);
                });

            migrationBuilder.CreateTable(
                name: "vehicle_life_event_wash",
                columns: table => new
                {
                    vehicle_life_event_wash_id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    date_time = table.Column<DateTime>(type: "datetime", nullable: false),
                    user_id = table.Column<int>(nullable: false),
                    vehicle_life_event_id = table.Column<int>(nullable: false),
                    comment = table.Column<string>(maxLength: 50, nullable: false),
                    timestamp = table.Column<byte[]>(rowVersion: true, nullable: false),
                    version_number = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_vehicle_life_event_wash", x => x.vehicle_life_event_wash_id);
                });

            migrationBuilder.CreateTable(
                name: "holding",
                columns: table => new
                {
                    holding_id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    holding_number = table.Column<string>(maxLength: 50, nullable: false),
                    address = table.Column<string>(maxLength: 100, nullable: false),
                    postcode = table.Column<string>(maxLength: 50, nullable: false),
                    grid_reference = table.Column<string>(maxLength: 50, nullable: false),
                    contact_id = table.Column<int>(nullable: false),
                    register_for_pigs = table.Column<bool>(nullable: false),
                    register_for_sheep_goats = table.Column<bool>(nullable: false),
                    register_for_cows = table.Column<bool>(nullable: false),
                    register_for_fish = table.Column<bool>(nullable: false),
                    register_for_poultry = table.Column<bool>(nullable: false),
                    comment = table.Column<string>(maxLength: 50, nullable: false),
                    timestamp = table.Column<byte[]>(rowVersion: true, nullable: false),
                    version_number = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_holding", x => x.holding_id);
                    table.ForeignKey(
                        name: "FK_holding_contact",
                        column: x => x.contact_id,
                        principalTable: "contact",
                        principalColumn: "contact_id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "critter_image_variant",
                columns: table => new
                {
                    critter_image_variant_id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    critter_image_original_id = table.Column<int>(nullable: false),
                    critter_image_modified_id = table.Column<int>(nullable: false),
                    width = table.Column<int>(nullable: false),
                    height = table.Column<int>(nullable: false),
                    timestamp = table.Column<byte[]>(rowVersion: true, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_critter_image_variant", x => x.critter_image_variant_id);
                    table.ForeignKey(
                        name: "FK_critter_image_variant_critter_image2",
                        column: x => x.critter_image_modified_id,
                        principalTable: "critter_image",
                        principalColumn: "critter_image_id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_critter_image_variant_critter_image",
                        column: x => x.critter_image_original_id,
                        principalTable: "critter_image",
                        principalColumn: "critter_image_id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "breed",
                columns: table => new
                {
                    breed_id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    critter_type_id = table.Column<int>(nullable: false),
                    description = table.Column<string>(maxLength: 50, nullable: false),
                    registerable = table.Column<bool>(nullable: false),
                    breed_society_contact_id = table.Column<int>(nullable: false),
                    comment = table.Column<string>(maxLength: 50, nullable: false),
                    timestamp = table.Column<byte[]>(rowVersion: true, nullable: false),
                    version_number = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_breed", x => x.breed_id);
                    table.ForeignKey(
                        name: "FK_breed_contact",
                        column: x => x.breed_society_contact_id,
                        principalTable: "contact",
                        principalColumn: "contact_id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_breed_critter_type",
                        column: x => x.critter_type_id,
                        principalTable: "critter_type",
                        principalColumn: "critter_type_id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "poultry_classification",
                columns: table => new
                {
                    poultry_classification_id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    critter_type_id = table.Column<int>(nullable: false),
                    comment = table.Column<string>(maxLength: 50, nullable: false),
                    timestamp = table.Column<byte[]>(rowVersion: true, nullable: false),
                    version_number = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_poultry_classification", x => x.poultry_classification_id);
                    table.ForeignKey(
                        name: "FK_poultry_classification_critter_type",
                        column: x => x.critter_type_id,
                        principalTable: "critter_type",
                        principalColumn: "critter_type_id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "product",
                columns: table => new
                {
                    product_id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    enum_product_type_id = table.Column<int>(nullable: false),
                    description = table.Column<string>(maxLength: 100, nullable: false),
                    default_volume = table.Column<double>(nullable: false),
                    requires_refridgeration = table.Column<bool>(nullable: false),
                    comment = table.Column<string>(maxLength: 50, nullable: false),
                    timestamp = table.Column<byte[]>(rowVersion: true, nullable: false),
                    version_number = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_product", x => x.product_id);
                    table.ForeignKey(
                        name: "FK_product_enum_product_type",
                        column: x => x.enum_product_type_id,
                        principalTable: "enum_product_type",
                        principalColumn: "enum_product_type_id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "menu_header",
                columns: table => new
                {
                    menu_header_id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    name = table.Column<string>(maxLength: 50, nullable: false),
                    title = table.Column<string>(maxLength: 50, nullable: false),
                    application_code = table.Column<int>(nullable: false),
                    role_id = table.Column<int>(nullable: false),
                    menu_header_parent_id = table.Column<int>(nullable: false),
                    image_uri = table.Column<string>(maxLength: 255, nullable: false),
                    comment = table.Column<string>(maxLength: 50, nullable: false),
                    timestamp = table.Column<byte[]>(rowVersion: true, nullable: false),
                    version_number = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_menu_header", x => x.menu_header_id);
                    table.ForeignKey(
                        name: "FK_menu_header_menu_header",
                        column: x => x.menu_header_parent_id,
                        principalTable: "menu_header",
                        principalColumn: "menu_header_id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_menu_header_role",
                        column: x => x.role_id,
                        principalTable: "role",
                        principalColumn: "role_id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "vehicle_trailer_map",
                columns: table => new
                {
                    vehicle_trailer_map_id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    vehicle_main_id = table.Column<int>(nullable: false),
                    trailer_id = table.Column<int>(nullable: false),
                    comment = table.Column<string>(maxLength: 50, nullable: false),
                    timestamp = table.Column<byte[]>(rowVersion: true, nullable: false),
                    version_number = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_vehicle_trailer_map", x => x.vehicle_trailer_map_id);
                    table.ForeignKey(
                        name: "FK_vehicle_trailer_map_vehicle1",
                        column: x => x.trailer_id,
                        principalTable: "vehicle",
                        principalColumn: "vehicle_id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_vehicle_trailer_map_vehicle",
                        column: x => x.vehicle_main_id,
                        principalTable: "vehicle",
                        principalColumn: "vehicle_id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "location",
                columns: table => new
                {
                    location_id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    name = table.Column<string>(maxLength: 50, nullable: false),
                    enum_location_type_id = table.Column<int>(nullable: false),
                    parent_id = table.Column<int>(nullable: false),
                    holding_id = table.Column<int>(nullable: false),
                    comment = table.Column<string>(maxLength: 50, nullable: false),
                    timestamp = table.Column<byte[]>(rowVersion: true, nullable: false),
                    version_number = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_location", x => x.location_id);
                    table.ForeignKey(
                        name: "FK_location_enum_location_type",
                        column: x => x.enum_location_type_id,
                        principalTable: "enum_location_type",
                        principalColumn: "enum_location_type_id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_location_holding",
                        column: x => x.holding_id,
                        principalTable: "holding",
                        principalColumn: "holding_id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_location_location",
                        column: x => x.parent_id,
                        principalTable: "location",
                        principalColumn: "location_id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "critter",
                columns: table => new
                {
                    critter_id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    critter_type_id = table.Column<int>(nullable: false),
                    gender = table.Column<string>(unicode: false, maxLength: 1, nullable: false),
                    name = table.Column<string>(maxLength: 50, nullable: false),
                    mum_critter_id = table.Column<int>(nullable: false),
                    dad_critter_id = table.Column<int>(nullable: false),
                    mum_further = table.Column<string>(maxLength: 255, nullable: true),
                    dad_further = table.Column<string>(maxLength: 255, nullable: true),
                    owner_contact_id = table.Column<int>(nullable: false),
                    breed_id = table.Column<int>(nullable: false),
                    critter_image_id = table.Column<int>(nullable: true),
                    comment = table.Column<string>(maxLength: 50, nullable: true),
                    timestamp = table.Column<byte[]>(rowVersion: true, nullable: false),
                    version_number = table.Column<int>(nullable: false),
                    flags = table.Column<int>(nullable: false),
                    tag_number = table.Column<string>(maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_critter", x => x.critter_id);
                    table.ForeignKey(
                        name: "FK_critter_breed",
                        column: x => x.breed_id,
                        principalTable: "breed",
                        principalColumn: "breed_id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_critter_critter_image",
                        column: x => x.critter_image_id,
                        principalTable: "critter_image",
                        principalColumn: "critter_image_id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_critter_critter_type",
                        column: x => x.critter_type_id,
                        principalTable: "critter_type",
                        principalColumn: "critter_type_id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_critter_critter1",
                        column: x => x.dad_critter_id,
                        principalTable: "critter",
                        principalColumn: "critter_id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_critter_critter",
                        column: x => x.mum_critter_id,
                        principalTable: "critter",
                        principalColumn: "critter_id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_critter_contact",
                        column: x => x.owner_contact_id,
                        principalTable: "contact",
                        principalColumn: "contact_id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "menu_header_item_map",
                columns: table => new
                {
                    menu_header_item_map_id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    menu_header_id = table.Column<int>(nullable: false),
                    menu_item_id = table.Column<int>(nullable: false),
                    comment = table.Column<string>(maxLength: 50, nullable: false),
                    timestamp = table.Column<byte[]>(rowVersion: true, nullable: false),
                    version_number = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_menu_header_item_map", x => x.menu_header_item_map_id);
                    table.ForeignKey(
                        name: "FK_menu_header_item_map_menu_header",
                        column: x => x.menu_header_id,
                        principalTable: "menu_header",
                        principalColumn: "menu_header_id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_menu_header_item_map_menu_item",
                        column: x => x.menu_item_id,
                        principalTable: "menu_item",
                        principalColumn: "menu_item_id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "vehicle_life_event",
                columns: table => new
                {
                    vehicle_life_event_id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    description = table.Column<string>(maxLength: 50, nullable: false),
                    date_time = table.Column<DateTime>(type: "datetime", nullable: false),
                    enum_vehicle_life_event_type_id = table.Column<int>(nullable: false),
                    vehicle_trailer_map_id = table.Column<int>(nullable: false),
                    comment = table.Column<string>(maxLength: 50, nullable: false),
                    timestamp = table.Column<byte[]>(rowVersion: true, nullable: false),
                    version_number = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_vehicle_life_event", x => x.vehicle_life_event_id);
                    table.ForeignKey(
                        name: "FK_vehicle_life_event_enum_vehicle_life_event_type",
                        column: x => x.enum_vehicle_life_event_type_id,
                        principalTable: "enum_vehicle_life_event_type",
                        principalColumn: "enum_vehicle_life_event_type_id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_vehicle_life_event_vehicle_trailer_map",
                        column: x => x.vehicle_trailer_map_id,
                        principalTable: "vehicle_trailer_map",
                        principalColumn: "vehicle_trailer_map_id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "product_purchase",
                columns: table => new
                {
                    product_purchase_id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    date_time = table.Column<DateTime>(type: "datetime", nullable: false),
                    product_id = table.Column<int>(nullable: false),
                    supplier_id = table.Column<int>(nullable: false),
                    location_id = table.Column<int>(nullable: false),
                    expiry = table.Column<DateTime>(type: "date", nullable: false),
                    cost = table.Column<decimal>(type: "money", nullable: false),
                    batch_number = table.Column<string>(maxLength: 50, nullable: false),
                    volume = table.Column<double>(nullable: false),
                    comment = table.Column<string>(maxLength: 50, nullable: false),
                    timestamp = table.Column<byte[]>(rowVersion: true, nullable: false),
                    version_number = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_product_purchase", x => x.product_purchase_id);
                    table.ForeignKey(
                        name: "FK_product_purchase_location",
                        column: x => x.location_id,
                        principalTable: "location",
                        principalColumn: "location_id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_product_purchase_product",
                        column: x => x.product_id,
                        principalTable: "product",
                        principalColumn: "product_id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_product_purchase_supplier",
                        column: x => x.supplier_id,
                        principalTable: "contact",
                        principalColumn: "contact_id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "critter_life_event",
                columns: table => new
                {
                    critter_life_event_id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    description = table.Column<string>(maxLength: 50, nullable: false),
                    date_time = table.Column<DateTime>(type: "datetime", nullable: false),
                    enum_critter_life_event_type_id = table.Column<int>(nullable: false),
                    enum_critter_life_event_data_id = table.Column<int>(nullable: false),
                    critter_id = table.Column<int>(nullable: false),
                    comment = table.Column<string>(maxLength: 50, nullable: false),
                    timestamp = table.Column<byte[]>(rowVersion: true, nullable: false),
                    version_number = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_critter_life_event", x => x.critter_life_event_id);
                    table.ForeignKey(
                        name: "FK_critter_life_event_critter",
                        column: x => x.critter_id,
                        principalTable: "critter",
                        principalColumn: "critter_id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_critter_life_event_enum_critter_life_event_type",
                        column: x => x.enum_critter_life_event_type_id,
                        principalTable: "enum_critter_life_event_type",
                        principalColumn: "enum_critter_life_event_type_id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "tag",
                columns: table => new
                {
                    tag_id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    critter_id = table.Column<int>(nullable: false),
                    tag = table.Column<string>(maxLength: 50, nullable: false),
                    rfid = table.Column<string>(maxLength: 255, nullable: false),
                    date_time = table.Column<DateTime>(type: "datetime", nullable: false),
                    user_id = table.Column<int>(nullable: false),
                    comment = table.Column<string>(maxLength: 50, nullable: false),
                    timestamp = table.Column<byte[]>(rowVersion: true, nullable: false),
                    version_number = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tag", x => x.tag_id);
                    table.ForeignKey(
                        name: "FK_tag_critter",
                        column: x => x.critter_id,
                        principalTable: "critter",
                        principalColumn: "critter_id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_admm_group_map_user_data_ids",
                table: "admm_group_map",
                columns: new[] { "group_entity_user_id", "group_entity_data_id" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_admm_group_map_user_datatypes",
                table: "admm_group_map",
                columns: new[] { "group_entity_user_id", "group_entity_data_type" });

            migrationBuilder.CreateIndex(
                name: "IX_admm_group_map_id",
                table: "admm_group_map",
                columns: new[] { "group_entity_user_id", "group_entity_data_type", "group_entity_data_id" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_breed_breed_society_contact_id",
                table: "breed",
                column: "breed_society_contact_id");

            migrationBuilder.CreateIndex(
                name: "IX_breed_critter_type_id",
                table: "breed",
                column: "critter_type_id");

            migrationBuilder.CreateIndex(
                name: "IX_critter_breed_id",
                table: "critter",
                column: "breed_id");

            migrationBuilder.CreateIndex(
                name: "IX_critter_critter_image_id",
                table: "critter",
                column: "critter_image_id");

            migrationBuilder.CreateIndex(
                name: "IX_critter_critter_type_id",
                table: "critter",
                column: "critter_type_id");

            migrationBuilder.CreateIndex(
                name: "IX_critter_dad_critter_id",
                table: "critter",
                column: "dad_critter_id");

            migrationBuilder.CreateIndex(
                name: "IX_critter_mum_critter_id",
                table: "critter",
                column: "mum_critter_id");

            migrationBuilder.CreateIndex(
                name: "IX_critter_owner_contact_id",
                table: "critter",
                column: "owner_contact_id");

            migrationBuilder.CreateIndex(
                name: "IX_UNIQ_tag_number",
                table: "critter",
                column: "tag_number",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_critter_image_variant_critter_image_modified_id",
                table: "critter_image_variant",
                column: "critter_image_modified_id");

            migrationBuilder.CreateIndex(
                name: "IX_critter_image_variant_id_pairs",
                table: "critter_image_variant",
                columns: new[] { "critter_image_original_id", "critter_image_modified_id" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_critter_image_variant_id_size",
                table: "critter_image_variant",
                columns: new[] { "critter_image_original_id", "width", "height" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_critter_life_event_critter_id",
                table: "critter_life_event",
                column: "critter_id");

            migrationBuilder.CreateIndex(
                name: "IX_critter_life_event_enum_critter_life_event_type_id",
                table: "critter_life_event",
                column: "enum_critter_life_event_type_id");

            migrationBuilder.CreateIndex(
                name: "IX_holding_contact_id",
                table: "holding",
                column: "contact_id");

            migrationBuilder.CreateIndex(
                name: "IX_location_enum_location_type_id",
                table: "location",
                column: "enum_location_type_id");

            migrationBuilder.CreateIndex(
                name: "IX_location_holding_id",
                table: "location",
                column: "holding_id");

            migrationBuilder.CreateIndex(
                name: "IX_location_parent_id",
                table: "location",
                column: "parent_id");

            migrationBuilder.CreateIndex(
                name: "IX_menu_header_menu_header_parent_id",
                table: "menu_header",
                column: "menu_header_parent_id");

            migrationBuilder.CreateIndex(
                name: "IX_menu_header_role_id",
                table: "menu_header",
                column: "role_id");

            migrationBuilder.CreateIndex(
                name: "IX_menu_header_item_map_menu_header_id",
                table: "menu_header_item_map",
                column: "menu_header_id");

            migrationBuilder.CreateIndex(
                name: "IX_menu_header_item_map_menu_item_id",
                table: "menu_header_item_map",
                column: "menu_item_id");

            migrationBuilder.CreateIndex(
                name: "IX_poultry_classification_critter_type_id",
                table: "poultry_classification",
                column: "critter_type_id");

            migrationBuilder.CreateIndex(
                name: "IX_product_enum_product_type_id",
                table: "product",
                column: "enum_product_type_id");

            migrationBuilder.CreateIndex(
                name: "IX_product_purchase_location_id",
                table: "product_purchase",
                column: "location_id");

            migrationBuilder.CreateIndex(
                name: "IX_product_purchase_product_id",
                table: "product_purchase",
                column: "product_id");

            migrationBuilder.CreateIndex(
                name: "IX_product_purchase_supplier_id",
                table: "product_purchase",
                column: "supplier_id");

            migrationBuilder.CreateIndex(
                name: "IX_tag_critter_id",
                table: "tag",
                column: "critter_id");

            migrationBuilder.CreateIndex(
                name: "IX_vehicle_life_event_enum_vehicle_life_event_type_id",
                table: "vehicle_life_event",
                column: "enum_vehicle_life_event_type_id");

            migrationBuilder.CreateIndex(
                name: "IX_vehicle_life_event_vehicle_trailer_map_id",
                table: "vehicle_life_event",
                column: "vehicle_trailer_map_id");

            migrationBuilder.CreateIndex(
                name: "IX_vehicle_trailer_map_trailer_id",
                table: "vehicle_trailer_map",
                column: "trailer_id");

            migrationBuilder.CreateIndex(
                name: "IX_vehicle_trailer_map_vehicle_main_id",
                table: "vehicle_trailer_map",
                column: "vehicle_main_id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "admm_group_map");

            migrationBuilder.DropTable(
                name: "admu_group");

            migrationBuilder.DropTable(
                name: "al_user_info");

            migrationBuilder.DropTable(
                name: "critter_image_variant");

            migrationBuilder.DropTable(
                name: "critter_life_event");

            migrationBuilder.DropTable(
                name: "critter_life_event_datetime");

            migrationBuilder.DropTable(
                name: "enum_critter_life_event_category");

            migrationBuilder.DropTable(
                name: "menu_header_item_map");

            migrationBuilder.DropTable(
                name: "poultry_classification");

            migrationBuilder.DropTable(
                name: "product_purchase");

            migrationBuilder.DropTable(
                name: "tag");

            migrationBuilder.DropTable(
                name: "vehicle_life_event");

            migrationBuilder.DropTable(
                name: "vehicle_life_event_wash");

            migrationBuilder.DropTable(
                name: "enum_critter_life_event_type");

            migrationBuilder.DropTable(
                name: "menu_header");

            migrationBuilder.DropTable(
                name: "menu_item");

            migrationBuilder.DropTable(
                name: "location");

            migrationBuilder.DropTable(
                name: "product");

            migrationBuilder.DropTable(
                name: "critter");

            migrationBuilder.DropTable(
                name: "enum_vehicle_life_event_type");

            migrationBuilder.DropTable(
                name: "vehicle_trailer_map");

            migrationBuilder.DropTable(
                name: "role");

            migrationBuilder.DropTable(
                name: "enum_location_type");

            migrationBuilder.DropTable(
                name: "holding");

            migrationBuilder.DropTable(
                name: "enum_product_type");

            migrationBuilder.DropTable(
                name: "breed");

            migrationBuilder.DropTable(
                name: "critter_image");

            migrationBuilder.DropTable(
                name: "vehicle");

            migrationBuilder.DropTable(
                name: "contact");

            migrationBuilder.DropTable(
                name: "critter_type");
        }
    }
}
