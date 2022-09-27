using System.Threading;

namespace DiningPhilosophers;

public class Program
{
	static void Main(string[] args)
	{

		var diningPhilosophers = new DiningPhilosophers();
		Thread philosopher0 = new Thread(new ParameterizedThreadStart(diningPhilosophers.Run));
		Thread philosopher1 = new Thread(new ParameterizedThreadStart(diningPhilosophers.Run));
		Thread philosopher2 = new Thread(new ParameterizedThreadStart(diningPhilosophers.Run));
		Thread philosopher3 = new Thread(new ParameterizedThreadStart(diningPhilosophers.Run));
		Thread philosopher4 = new Thread(new ParameterizedThreadStart(diningPhilosophers.Run));
		philosopher0.Start(0);
		philosopher1.Start(1);
		philosopher2.Start(2);
		philosopher3.Start(3);
		philosopher4.Start(4);


	}
}