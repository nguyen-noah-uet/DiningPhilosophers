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

public class Philosopher
{
	public int Id { get; set; }
	public int? LeftChopstick { get; set; } = null;
	public int? RightChopstick { get; set; } = null;
	public int ExpectedLeftChopstick => (Id + 1) % 5;
	public int ExpectedRightChopstick => Id;
	public bool IsEating => LeftChopstick != null && RightChopstick != null;
}