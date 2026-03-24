using System;
using System.Diagnostics;
using System.Threading.Tasks;

using ClassLibrary2;


unsafe class Program
{
	static void Main()
	{
		Stopwatch t = Stopwatch.StartNew();

		ushort threads = 1_000;
		ThreadPool.SetMinThreads(threads, threads);

		const string task = "Fpk3crp6SqKyu7RBCYRdVJFl+53cJqqMxJeJHuDyVKfvqVQcC1kh4Gq0LzepYdfQdKidpTT9DaewWfy4nd5Dow8xp7Ub7O+VmCRFAhAGWJoWxvg0yBkxfBxMw9Qgn9w2umJ/V2GU/6IwO9NVT0IzLcYWYzsJcfe3wFd8Hi8MCbntqFtkIT7TT/rCFfsD8hTO";

		const uint N_26_POW_4 = 456_976;
		const byte LEN = 26;
		const ushort LEN_POW_2 = 676;
		const ushort LEN_POW_3 = 17_576;
		const byte UNI_A = 65;
		const byte BUFFSIZE = 4;

		Parallel.For(0, N_26_POW_4, new ParallelOptions {MaxDegreeOfParallelism = threads}, (i, state) => {
			char* buff = stackalloc char[BUFFSIZE];

			buff[3] = (char)(i%LEN + UNI_A);
			buff[2] = (char)(i/LEN%LEN + UNI_A);
			buff[1] = (char)(i/LEN_POW_2%LEN + UNI_A);
			buff[0] = (char)(i/LEN_POW_3 + UNI_A);

			string key = new string(buff, 0, BUFFSIZE);

			Cryptor cryptor = new Cryptor();

			if (cryptor.CorrectCode(task, key)) {
				string result = cryptor.Decoding(task, key);
				if (result.Contains("РАСШИФРОВАННО")) {
					Console.WriteLine($"key: {key}\ntext: {result}");
					state.Stop();
				}
			}
		});

		Console.WriteLine($"Time: {t.Elapsed.TotalSeconds}");
	}
}
