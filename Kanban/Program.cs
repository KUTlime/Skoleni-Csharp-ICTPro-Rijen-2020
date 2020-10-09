using System;
using System.Collections.Generic;
using System.Linq;
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
			// Ukázka vytvoření testovacího nastavení.
			var test = new KanbanBoardSettings()
			{
				DefaultDueToDays = 14,
				DefaultTaskDescription = "Empty description",
				DefaultColumnSetup = new List<TaskStatus>()
				{
					new TaskStatus(){ Value = "Má se udělat"},
					new TaskStatus(){ Value = "Hotovo"}
				}

			};
			var testKanbaBoard = new KanbanBoard(test);

			// Vytvoření kanban boardu jako takového.
			var kanbanBoard = new KanbanBoard(new KanbanBoardSettings() { DefaultDueToDays = 14 });
			// Vytvoření úkolů
			kanbanBoard.CreateTask();
			kanbanBoard.CreateTask("Improve kanban", "Cleanup code", "Doing");
			kanbanBoard.CreateTask("Improve due date", "Cleanup code", "Doing");
			// Úprava nastavení úkolů
			kanbanBoard.TaskList[2].DueTo = DateTime.UtcNow.AddDays(30);
			// Přidání nastavení úkolů
			kanbanBoard.CreateTask();
			// Posunutí do jiného sloupce.
			kanbanBoard.TaskList[3].TaskStatus.Value = "Completed";
			// Výpis všech úkolů
			Console.WriteLine(string.Join("\n", kanbanBoard.TaskList));
			// Výpis úkolů ve stavu To-Do
			kanbanBoard.PrintTask("To-Do");
			// Výpis úkolů ve stavu Doing
			kanbanBoard.PrintTask("Doing");
			// Výpis úkolů ve stavu Completed
			kanbanBoard.PrintTask("Completed");
			// Přidání nového sloupce
			kanbanBoard.CreateTask("Prodloužit občanku", "V roce 2025 si nechat prodloužit občanku", "2025");
			kanbanBoard.PrintTask("2025");
			// Zjištění všech unikátních sloupců
			Console.WriteLine(string.Join("\n", kanbanBoard.TaskList.Select(tl => tl.TaskStatus.Value).Distinct()));
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
		// Implementace chain-of-responsibility návrhového vzoru

		/// <summary>
		/// Tato metoda nepřebírá žádné parametry.
		/// Volání je delegováno do přetížené metody, která přebírá o jeden parametr navíc,
		/// konkrétně výchozí jméno úkolu.
		/// Metoda dělá pouze jednu věc - doplňuje výchozí jméno úkolu.
		/// </summary>
		public void CreateTask()
		{
			CreateTask(KanbanSetting.DefaultTaskName);
		}

		/// <summary>
		/// Tato metoda přebírá jeden argument.
		/// Volání je delegováno do přetížené metody, která přebírá o jeden parametr navíc,
		/// konkrétně výchozí popis úkolu.
		/// Metoda dělá pouze jednu věc - doplňuje výchozí popis úkolu.
		/// </summary>
		public void CreateTask(string taskName)
		{
			CreateTask(taskName, KanbanSetting.DefaultTaskDescription);
		}

		/// <summary>
		/// Tato metoda dělá pouze jednu věc - doplňuje výchozí status.
		/// </summary>
		/// <param name="taskName"></param>
		/// <param name="taskDescription"></param>
		public void CreateTask(string taskName, string taskDescription)
		{
			CreateTask(taskName, taskDescription, KanbanSetting.DefaultColumnSetup[0].Value);
		}

		/// <summary>
		/// Metoda dělá pouze jednu věc - doplňuje výchozí dobu ukončení úkolu.
		/// </summary>
		/// <param name="taskName"></param>
		/// <param name="taskDescription"></param>
		/// <param name="taskStatus"></param>
		public void CreateTask(string taskName, string taskDescription, string taskStatus)
		{
			CreateTask(taskName, taskDescription, taskStatus, KanbanSetting.DefaultDueToDays);
		}

		/// <summary>
		/// Metoda dělá pouze jednu věc - reálně vytvoří ten task.
		/// </summary>
		/// <param name="taskName"></param>
		/// <param name="taskDescription"></param>
		/// <param name="taskStatus"></param>
		/// <param name="dueToDays"></param>
		public void CreateTask(string taskName, string taskDescription, string taskStatus, int dueToDays)
		{
			TaskList.Add(
				new Task(new TaskName(taskName), new TaskDescription(taskDescription), dueToDays)
				{
					TaskStatus = new TaskStatus()
					{
						Value = taskStatus
					}
				}
			);
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
		/// <summary>
		/// Vytištění všech úkolů ze všech jednotlivých sloupců.
		/// V reálu bychom vrátili nějakou šikovnou kolekci jako
		/// slovník sloupců a seznam úkolů k tomu přiřazených.
		/// </summary>
		public void PrintTasks()
		{
			foreach (var column in TaskList.Select(task => task.TaskStatus.Value).Distinct())
			{
				PrintTask(column);
			}
		}

	}

	/// <summary>
	/// Třída pro úkol.
	/// Třída má různé vlastnosti, půlka z nich je inicializována v konstruktoru,
	/// tři další jsou inicializovány výchozí hodnotou samotné vlastnosti.
	/// Zlaté pravidlo programování v C#, co lze vyřešit výchozí hodnotou vlastnosti,
	/// měli byste vyřešit výchozí hodnotou vlastnosti.
	/// </summary>
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

	/// <summary>
	/// Jednuchá třída, která poskytuje Id.
	/// Kterákoliv třída, která z této třídy dědí,
	/// má k dispozici vlastnost Id.
	/// </summary>
	class Entity
	{
		public Guid Id { get; } = Guid.NewGuid();
	}

	/// <summary>
	/// Obal pro jméno úkolu.
	/// Třídy drží string s konkrétní hodnotou.
	/// Pokud by byly nějaké byznys požadavky
	/// na název úkolu, tak budou zapracovány sem.
	/// </summary>
	class TaskName
	{
		public TaskName(string value)
		{
			Value = value;
		}
		public string Value { get; set; }
	}

	/// <summary>
	/// Obal pro popis úkolu.
	/// Třídy drží string s konkrétní hodnotou.
	/// Pokud by byly nějaké byznys požadavky
	/// na popis úkolu, tak budou zapracovány sem.
	/// </summary>
	class TaskDescription
	{
		public TaskDescription(string value)
		{
			Value = value;
		}
		public string Value { get; set; }
	}

	/// <summary>
	/// Obal pro status úkolu.
	/// Třídy drží string s konkrétní hodnotou.
	/// Pokud by byly nějaké byznys požadavky
	/// na status úkolu, tak budou zapracovány sem.
	/// Třída nemá konstruktor, ale hodnota má
	/// veřejný get i set, takže můžeme pohodlně pracovat
	/// s hodnotou toho statusu.
	/// </summary>
	class TaskStatus
	{
		public string Value { get; set; }
	}

	/// <summary>
	/// Pro potřeby nastavení nového Kanban Boardu
	/// můžeme veškeré potřebné nastavení vložit do
	/// této třídy.
	/// Třída má tzn. implicitní konstruktor, tj.
	/// konstruktor, který je vytvořen automaticky při
	/// překladu zdrojového kódu.
	/// </summary>
	class KanbanBoardSettings
	{
		/*
		// Implicitní konstruktor, který je vložen automaticky.
		public KanbanBoardSettings(){}
		*/
		public int DefaultDueToDays { get; set; } = 7;
		public List<TaskStatus> DefaultColumnSetup { get; set; } = new List<TaskStatus>
		{
			new TaskStatus() { Value = "To-Do" },
			new TaskStatus() { Value = "Doing" },
			new TaskStatus() { Value = "Completed" }
		};
		public string DefaultTaskDescription { get; set; } = "";
		public string DefaultTaskName { get; set; } = "New task";

	}
}
