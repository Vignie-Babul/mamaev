// run without `ConsoleApp5.csproj`: `dotnet run -p:AllowUnsafeBlocks=true Program.cs` (run with unsafe flag). 
// run with    `ConsoleApp5.csproj`: `dotnet run Program.cs`                           (unsafe flag in the .csproj)


using System;
using System.Threading.Tasks;

using ClassLibrary2;


unsafe class Program
{
	static void Main()
	{
		ushort threads = 1_000;
		ThreadPool.SetMinThreads(threads, threads);

		// №7
		const string task = "Fpk3crp6SqKyu7RBCYRdVJFl+53cJqqMxJeJHuDyVKfvqVQcC1kh4Gq0LzepYdfQdKidpTT9DaewWfy4nd5Dow8xp7Ub7O+VmCRFAhAGWJoWxvg0yBkxfBxMw9Qgn9w2umJ/V2GU/6IwO9NVT0IzLcYWYzsJcfe3wFd8Hi8MCbntqFtkIT7TT/rCFfsD8hTO";

		const uint N_26_POW_4 = 456_976;

		const byte LEN = 26;
		const ushort LEN_POW_2 = 676;
		const ushort LEN_POW_3 = 17_576;

		const byte UNICODE_A = 65;
		const byte BUFFSIZE = 4;

		string result_key = "\0";
		string result_task = "\0";

		Parallel.For(0, N_26_POW_4, new ParallelOptions {MaxDegreeOfParallelism = threads}, (i, state) => {
			char* buff = stackalloc char[BUFFSIZE];

			buff[3] = (char)(i%LEN + UNICODE_A);
			buff[2] = (char)(i/LEN%LEN + UNICODE_A);
			buff[1] = (char)(i/LEN_POW_2%LEN + UNICODE_A);
			buff[0] = (char)(i/LEN_POW_3 + UNICODE_A);

			string key = new string(buff, 0, BUFFSIZE);

			Cryptor cryptor = new Cryptor();

			try {
				string result = cryptor.Decoding(task, key);
				if (result.Contains("РАСШИФРОВАННО")) {
					Console.WriteLine($"key: {key}\ntext: {result}");
					result_key = key;
					result_task = result;
					state.Stop();
				}
			} catch {}
		});

		Console.WriteLine($"\n\nkey: {result_key}\ntext: {result_task}");
	}
}
