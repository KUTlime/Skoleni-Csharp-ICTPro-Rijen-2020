using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Kanban
{
	class Program
	{
		// Naprogramujte knihovnu, která umožní implementaci Kanban Boardu.
		// Knihovna by měla obsahovat třídy, které umožní vytvořit Kanban Board,
		// přidat úkoly, přesunout úkoly mezi jednotlivými sloupci.
		static void Main(string[] args)
		{
			var kanbanBoard = new KanbanBoard(new KanbanBoardSettings() { DefaultDueToDays = 14});
			kanbanBoard.CreateTask();
			kanbanBoard.CreateTask("Improve kanban", "Cleanup code", "Doing");
			kanbanBoard.CreateTask("Improve due date", "Cleanup code", "Doing");
			kanbanBoard.TaskList[2].DueTo = DateTime.UtcNow.AddDays(30);
			kanbanBoard.CreateTask();
			kanbanBoard.TaskList[3].TaskStatus.Value = "Completed";
			Console.WriteLine(string.Join("\n", kanbanBoard.TaskList));
			kanbanBoard.PrintTask("To-Do");
			kanbanBoard.PrintTask("Doing");
			kanbanBoard.PrintTask("Completed");
		}
	}

	class KanbanBoard
	{
		public KanbanBoard(KanbanBoardSettings kanbanSetting)
		{
			KanbanSetting = kanbanSetting;
		}
		public KanbanBoardSettings KanbanSetting { get; set; }
		public List<Task> TaskList { get; set; } = new List<Task>();
		public void CreateTask(string taskName, string taskDescription, string taskStatus)
		{
			TaskList.Add(
			new Task(new TaskName(taskName), new TaskDescription(taskDescription), KanbanSetting.DefaultDueToDays)
			{ TaskStatus = new TaskStatus() { Value = taskStatus} }
			);
		}
		public void CreateTask()
		{
			CreateTask(KanbanSetting.DefaultTaskName,
				KanbanSetting.DefaultTaskDescription,
				KanbanSetting.DefaultColumnSetup[0].Value);
		}
		public void PrintTask(string status)
		{
			Console.WriteLine($"Task in {status}");
			foreach (var task in TaskList)
			{
				if (task.TaskStatus.Value == status)
				{
					Console.WriteLine(task);
				}
			}
		}

	}

	class Task : Entity
	{
		public Task(TaskName taskName, TaskDescription taskDescription, int defaultDueToDays = 7)
		{
			TaskName = taskName;
			TaskDescription = taskDescription;
			DueTo = DateTime.UtcNow.AddDays(defaultDueToDays);
		}
		public TaskStatus TaskStatus { get; set; } = new TaskStatus() { Value = "To-Do" };
		public TaskName TaskName { get; set; }
		public TaskDescription TaskDescription { get; set; }
		public DateTime Created { get; } = DateTime.UtcNow;
		public DateTime LastChanged { get; private set; } = DateTime.UtcNow;
		public DateTime DueTo { get; set; }

		public override string ToString()
		{
			return $"Task ID: {Id}, Name: {TaskName.Value}, status {TaskStatus.Value}, due to {DueTo}";
		}
	}

	class Entity
	{
		public Guid Id { get; } = Guid.NewGuid();
	}

	class TaskName
	{
		public TaskName(string value)
		{
			Value = value;
		}
		public string Value { get; set; }
	}

	class TaskDescription
	{
		public TaskDescription(string value)
		{
			Value = value;
		}
		public string Value { get; set; }
	}

	class TaskStatus
	{
		public string Value { get; set; }
	}

	class KanbanBoardSettings
	{
		public int DefaultDueToDays { get; set; } = 7;
		public TaskStatus[] DefaultColumnSetup { get; set; } = new TaskStatus[] { new TaskStatus() { Value = "To-Do" }, new TaskStatus() { Value = "Doing" }, new TaskStatus() { Value = "Completed" } };
		public string DefaultTaskDescription { get; set; } = "";
		public string DefaultTaskName { get; set; } = "New task";

	}
}
