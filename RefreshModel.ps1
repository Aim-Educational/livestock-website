$SERVER_NAME	= "SEAL-WIN64"
$INSTANCE_NAME	= "SQLEXPRESS"
$DATABASE_NAME  = "Livestock"
$PROJECT		= "Database"
$OUTPUT_DIR     = "Models"
$CONNECTION     = "Server="+$SERVER_NAME+"\"+$INSTANCE_NAME+";Database="+$DATABASE_NAME+";Trusted_Connection=True;"

$PROJECT_PATH   = Get-Project $PROJECT
$PROJECT_PATH   = [System.IO.Path]::GetDirectoryName($PROJECT_PATH.FullName)

$MODEL_PATH     = [System.IO.Path]::Combine($PROJECT_PATH, $OUTPUT_DIR)

$CONTEXT_NAME   = "LivestockContext"
$CONTEXT_PATH   = [System.IO.Path]::Combine($MODEL_PATH, "$CONTEXT_NAME.cs")

$TABLE_OBJECT_PROPERTY_REGEX        = [regex]"public\s[^\s\<]+\s([^\s]+)\s{\s[gs]et;\s[gs]et;\s}"
$TABLE_OBJECT_NAME_TRUNCATE_MATCHES = "* Contact Id","* Id"

# Reverse-engineer the database.
Scaffold-DbContext $CONNECTION Microsoft.EntityFrameworkCore.SqlServer -OutputDir $OUTPUT_DIR -Project $PROJECT -Force

# Patch the DbContext
[System.Collections.ArrayList]$DbContextContent = [System.IO.File]::ReadAllLines($CONTEXT_PATH);
for ($i = 0; $i -lt $DbContextContent.Count; $i++) 
{
    $line = $DbContextContent[$i]
    if ($line -like "*public partial class LivestockContext*") 
    {
        $i++
        $i++
        $DbContextContent.Insert($i++, "public string db;")
        $DbContextContent.Insert($i++, "public LivestockContext(string db){ this.db = db; }")
        continue
    }

    if($line -like "*optionsBuilder.UseSqlServer*")
    {
        $DbContextContent[$i] = "optionsBuilder.UseSqlServer(this.db);"
        $DbContextContent.RemoveAt($i-1)
        break
    }
}
Write-Output For the love of god just work
Out-File -FilePath $CONTEXT_PATH -InputObject $([System.String]::Join("`n", $DbContextContent.ToArray())) -Encoding string

# Patch the table object files
$fileList = [System.IO.Directory]::EnumerateFiles($MODEL_PATH)

foreach ($filePath in $fileList)
{
    if($filePath -like "*$CONTEXT_NAME.cs*")
    {
        continue
    }

    Write-Output Processing: $filePath

    [System.Collections.ArrayList]$lines = [System.IO.File]::ReadAllLines($filePath);
    $lines.Insert(2, "using System.ComponentModel;")

    for ($i = 0; $i -lt $lines.Count; $i++)
    {
        # Check if the current line is a property
        $match = $TABLE_OBJECT_PROPERTY_REGEX.Match($lines[$i])
        if (-not $match.Success)
        {
            continue
        }

        $name = $match.Groups[1].Value
        $newName = ""

        # Add a space between individual words
        foreach ($char in $name.ToCharArray())
        {
            if ($newName.Length -eq 0)
            {
                $newName += $char
                continue
            }

            if ([System.Char]::IsUpper($char))
            {
                $newName += " "
                $newName += $char
                continue
            }

            $newName += $char
        }

        # Truncate everything
        foreach ($truncateMatch in $TABLE_OBJECT_NAME_TRUNCATE_MATCHES)
        {
            if ($newName -like $truncateMatch)
            {
                $newName = $newName.Substring(0, ($newName.Length - $truncateMatch.Length) + 1)
            }
        }

        $lines.Insert($i, "[DisplayName(`"$newName`")]")
        $i++
    }

    Out-File -FilePath $filePath -InputObject $([System.String]::Join("`n", $lines.ToArray())) -Encoding string
}