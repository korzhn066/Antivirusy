// See https://aka.ms/new-console-template for more information


using Antivirusy;
using Microsoft.EntityFrameworkCore;

var driverPathes = new List<string>();
var filePathes = new List<string>();  


foreach(var driver in DriveInfo.GetDrives())
{
    driverPathes.Add(driver.Name);
}

//тестировать удобнее, если всё не читает (слишком долго)
int max = 100;

void GetFilePahesInFolder(string folderPath)
{
    if (filePathes.Count == max)
        return;

    var allfiles = Directory.GetFiles(folderPath);
    foreach (var filename in allfiles)
    {
        if (filePathes.Count == max)
            return;

        filePathes.Add(filename);

        //Для многопоточного чтения файлов
        ThreadPool.QueueUserWorkItem(ReadFile, filename);
    }

    var allfolders = Directory.GetDirectories(folderPath);
    foreach (var folder in allfolders)
    {
        GetFilePahesInFolder(folder);
    }
}

// Долго все диски смотреть

/*foreach (var driverPath in driverPathes)
{
    GetFilePahesInFolder(driverPath);
}
*/

List<Signatura> signatures = new List<Signatura>();

void GetSignatures ()
{
    using(DBContext db = new())
    {
        signatures = db.Signaturas
            .Include(s => s.Bytees)
            .ToList();
    }
}

void Start()
{
    GetSignatures();
    GetFilePahesInFolder("D:\\antivirusy");
}

Start();
void ReadFile(object? state)
{
    if (state is null)
        throw new ArgumentNullException(nameof(state));

    string path = (string)state;

    foreach (var signatura in signatures)
    {
        using (FileStream fstream = File.OpenRead(path))
        {
            var bytes = new List<byte>();

            foreach (var bytee in signatura.Bytees)
            {
                bytes.Add(bytee.Data);
            }


            Console.WriteLine(signatures.Count);
            Console.WriteLine(BitConverter.ToString(bytes.ToArray()));

            while (fstream.Length >= fstream.Position)
            {
                var buffer = new byte[signatura.Bytees.Count];
                fstream.Read(buffer, 0, signatura.Bytees.Count);

                if (BitConverter.ToString(bytes.ToArray()) == BitConverter.ToString(buffer))
                {
                    Console.WriteLine("Find signatura: " + path);
                    return;
                }

                fstream.Seek(fstream.Position - signatura.Bytees.Count + 1, SeekOrigin.Begin);
            }
        }
    }

    
}



//подождать пока потоки доработают
while(ThreadPool.PendingWorkItemCount > 0) {}

Thread.Sleep(3000);
/*1.Получить список дисков. + 
2. Рекурсивно получить список папок и файлов для каждого диска. + 
3. Открыть каждый файл на чтение.
4. Считать каждый файл блоками и искать в блоках сигнатуру.
5. Сигнатура - это последовательность байт, которую мы считаем вредоносной.
Можно её найти с помощью шестнадцатеричного редактора (например, HxD https://mh-nexus.de/en/hxd/).
6. Если сигнатура в файле найдена, то вывести в логи, что файл вредоносный.*/