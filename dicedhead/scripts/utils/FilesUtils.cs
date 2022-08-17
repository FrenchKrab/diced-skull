using System.Collections.Generic;
using Godot;

public static class FilesUtils
{
    public static IEnumerable<string> ListAllFiles(string path)
    {
    //     func list_files_in_directory(path):
    // var files = []
    // var dir = Directory.new()
    // dir.open(path)
    // dir.list_dir_begin()

    // while true:
    //     var file = dir.get_next()
    //     if file == "":
    //         break
    //     elif not file.begins_with("."):
    //         files.append(file)

    // dir.list_dir_end()

    // return files

        var result = new List<string>();

        var dir = new Directory();
        dir.Open(path);
        dir.ListDirBegin(skipNavigational:true);
        while (true)
        {
            var fileName = dir.GetNext();
            if (string.IsNullOrEmpty(fileName))
                break;

            if (!dir.CurrentIsDir())
                result.Add(fileName);
        }

        return result;
    }
}