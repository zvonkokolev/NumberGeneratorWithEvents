﻿using System;

namespace NumberGenerator.Logic
{
	/// <summary>
	/// Beobachter, welcher die Zahlen auf der Konsole ausgibt.
	/// Diese Klasse dient als Basisklasse für komplexere Beobachter.
	/// </summary>
	public class BaseObserver
	{
		#region Fields

		protected readonly RandomNumberGenerator _numberGenerator;

		#endregion

		#region Properties

		public int CountOfNumbersReceived { get; protected set; }
		public int CountOfNumbersToWaitFor { get; protected set; }

		#endregion

		#region Constructors

		public BaseObserver(RandomNumberGenerator numberGenerator, int countOfNumbersToWaitFor)
		{
			if(numberGenerator == null)
			{
				throw new ArgumentException("NULL");
			}
			if (countOfNumbersToWaitFor < 0)
			{
				throw new ArgumentException("Zahl war negativ");
			}
			_numberGenerator = numberGenerator ?? throw new ArgumentNullException($"Das war keine Zahlengenerator");
			CountOfNumbersToWaitFor = countOfNumbersToWaitFor;
			_numberGenerator.NextNumberHandlerChanged += OnNextNumber;
		}

		#endregion

		#region Methods

		#region IObserver Members

		/// <summary>
		/// Wird aufgerufen wenn der NumberGenerator eine neue Zahl generiert hat.
		/// </summary>
		/// <param name="number"></param>
		public virtual void OnNextNumber(int number)
		{
			CountOfNumbersReceived++;
			// Sobald die Anzahl der max. Beobachtungen (_countOfNumbersToWaitFor) erreicht ist -> Detach()
			if (CountOfNumbersReceived >= CountOfNumbersToWaitFor)
			{
				Console.ForegroundColor = ConsoleColor.Red;
				Console.WriteLine($"   >> {this.GetType().Name}: Received '{CountOfNumbersReceived}' of '{CountOfNumbersToWaitFor}' => I am not interested in new numbers anymore => Detach().");
				Console.ResetColor();
				DetachFromNumberGenerator();
			}
		}

		#endregion

		public override string ToString()
		{
			return $"{GetType().Name}";
		}

		protected void DetachFromNumberGenerator()
		{
			_numberGenerator.NextNumberHandlerChanged -= OnNextNumber;
		}

		#endregion
	}
}
