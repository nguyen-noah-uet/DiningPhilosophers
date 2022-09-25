﻿using Serilog;
using Serilog.Events;
using System;
using System.Threading;
using System.Timers;

namespace DiningPhilosophers;

public class DiningPhilosophers
{
	private readonly Semaphore[] _chopsticks;
	private readonly Philosopher[] _philosophers;
	private readonly DateTime _start;
	private const int ChopsticksCount = 5;

	private ILogger _logger;
	private System.Timers.Timer timer;
	public DiningPhilosophers()
	{
		_chopsticks = new Semaphore[]
		{
			new Semaphore(1,1),
			new Semaphore(1,1),
			new Semaphore(1,1),
			new Semaphore(1,1),
			new Semaphore(1,1),
		};
		_philosophers = new Philosopher[]
		{
			new Philosopher() { Id = 0 },
			new Philosopher() { Id = 1 },
			new Philosopher() { Id = 2 },
			new Philosopher() { Id = 3 },
			new Philosopher() { Id = 4 }
		};
		timer = new System.Timers.Timer(5000)
		{
			AutoReset = true,
			Enabled = true,
		};
		timer.Elapsed += TimerOnElapsed;

		_logger = new LoggerConfiguration()
			.WriteTo.File("output.txt", rollingInterval: RollingInterval.Day)
			.WriteTo.Console()
			.CreateLogger();
		_start = DateTime.Now;
	}



	public void Run(object? num)
	{
		timer.Start();
		int id = (int)num;
		do
		{
			if (id % 2 == 1)
			{
				// wait the right chopstick
				_chopsticks[id].WaitOne();
				_philosophers[id].RightChopstick = id;
				// wait the left chopstick
				_chopsticks[(id + 1) % ChopsticksCount].WaitOne();
				_philosophers[id].LeftChopstick = (id + 1) % ChopsticksCount;
			}
			else
			{
				// wait the right chopstick
				_chopsticks[(id + 1) % ChopsticksCount].WaitOne();
				_philosophers[id].LeftChopstick = (id + 1) % ChopsticksCount;
				// wait the left chopstick
				_chopsticks[id].WaitOne();
				_philosophers[id].RightChopstick = id;
			}

			#region Eat

			int time = Random.Shared.Next(4500, 5500);
			Thread.Sleep(time);

			#endregion

			_chopsticks[id].Release();
			_philosophers[id].RightChopstick = null;
			_chopsticks[(id + 1) % ChopsticksCount].Release();
			_philosophers[id].LeftChopstick = null;



		} while ((DateTime.Now - _start).Seconds < 20);
		timer.Stop();
		timer.Dispose();
	}

	private void TimerOnElapsed(object? sender, ElapsedEventArgs e)
	{
		//Console.WriteLine("-----------------------------------------------");
		_logger.Write(LogEventLevel.Information, "-----------------------------------------------");
		foreach (Philosopher philosopher in _philosophers)
		{
			if (philosopher.IsEating)
			{
				// Console.WriteLine("Nhà triết học " + philosopher.Id + " đang ăn. Đũa trái: " + philosopher.LeftChopstick + ", đũa phải: " + philosopher.RightChopstick);
				_logger.Write(LogEventLevel.Information, "Nhà triết học " + philosopher.Id + " đang ăn. Đũa trái: " + philosopher.LeftChopstick + ", đũa phải: " + philosopher.RightChopstick);
			}
			else if (philosopher.LeftChopstick != null)
			{
				// Console.WriteLine("Nhà triết học " + philosopher.Id + " đang cầm đũa trái " + philosopher.LeftChopstick + ", chờ đũa phải " + philosopher.ExpectedRightChopstick);
				_logger.Write(LogEventLevel.Information, "Nhà triết học " + philosopher.Id + " đang cầm đũa trái " + philosopher.LeftChopstick + ", chờ đũa phải " + philosopher.ExpectedRightChopstick);

			}
			else if (philosopher.RightChopstick != null)
			{
				// Console.WriteLine("Nhà triết học " + philosopher.Id + " đang cầm đũa phải " + philosopher.RightChopstick + ", chờ đũa trái " + philosopher.ExpectedLeftChopstick);
				_logger.Write(LogEventLevel.Information, "Nhà triết học " + philosopher.Id + " đang cầm đũa phải " + philosopher.RightChopstick + ", chờ đũa trái " + philosopher.ExpectedLeftChopstick);
			}
			else
			{
				// Console.WriteLine("Nhà triết học " + philosopher.Id + " đang chờ đũa trái " + philosopher.ExpectedLeftChopstick + ", chờ đũa phải " + philosopher.ExpectedRightChopstick);
				_logger.Write(LogEventLevel.Information, "Nhà triết học " + philosopher.Id + " đang chờ đũa trái " + philosopher.ExpectedLeftChopstick + ", chờ đũa phải " + philosopher.ExpectedRightChopstick);

			}
		}
		//Console.WriteLine("-----------------------------------------------");
		_logger.Write(LogEventLevel.Information, "-----------------------------------------------");
	}
}