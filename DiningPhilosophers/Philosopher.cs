namespace DiningPhilosophers;

public class Philosopher
{
	private const int Count = 5;
	public int Id { get; set; }
	public int? LeftChopstick { get; set; } = null;
	public int? RightChopstick { get; set; } = null;
	public int ExpectedLeftChopstick => (Id + 1) % Count;
	public int ExpectedRightChopstick => Id;
	public bool IsEating => LeftChopstick != null && RightChopstick != null;
}