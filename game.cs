using System;
using System.Collections.Generic;
using System.Threading;
using ConsoleApp1;

namespace ConsoleApp2
{
  /// <summary>
  /// Окно.
  /// </summary>
  internal struct WindowRect
  {
    /// <summary>
    /// Левая координата.
    /// </summary>
    public int Left;

    /// <summary>
    /// Верхняя координата.
    /// </summary>
    public int Top;

    /// <summary>
    /// Ширина.
    /// </summary>
    public int Width;

    /// <summary>
    /// Высота.
    /// </summary>
    public int Height;
  }

  /// <summary>
  /// Игра.
  /// </summary>
  internal class Program
  {
    /// <summary>
    /// Шаг отступа одного элемента от предыдущего.
    /// </summary>
    private const int itemStep = 7;

    /// <summary>
    /// Первоначальное количество элементов.
    /// </summary>
    private const int initialItemsCount = 16;


    /// <summary>
    /// Максимально возможное количество выбираемых за раз элементов.
    /// </summary>
    private const int maxUserInput = 3;

    /// <summary>
    /// Время ожидания перед шагом компьютера.
    /// </summary>
    private const int MillisecondsTimeoutBeforeComputerStep = 1500;

    /// <summary>
    /// Главное окно.
    /// </summary>
    private readonly WindowRect mainWindow = new WindowRect { Left = 0, Top = 0, Width = 117, Height = 28 };

    /// <summary>
    /// Окно сообщений компьютера.
    /// </summary>
    private readonly WindowRect dialogWindow = new WindowRect { Left = 57, Top = 16, Width = 58, Height = 10 };

    /// <summary>
    /// Окно оставшихся элементов.
    /// </summary>
    private readonly WindowRect itemsWindow = new WindowRect { Left = 2, Top = 2, Width = 113, Height = 11 };

    /// <summary>
    /// Шаблон окна элемента.
    /// </summary>
    private readonly WindowRect itemTemplateWindow = new WindowRect { Left = 4, Top = 4, Width = 4, Height = 7 };

    /// <summary>
    /// Окно ввода пользователя.
    /// </summary>
    private readonly WindowRect userInputWindow = new WindowRect { Left = 2, Top = 16, Width = 20, Height = 10 };

    /// <summary>
    /// Количество оставшихся элементов
    /// </summary>
    private int itemsCount = initialItemsCount;

    /// <summary>
    /// Последний ввод пользователя.
    /// </summary>
    private int lastUserInput = 0;

    /// <summary>
    /// Запустить игру.
    /// </summary>
    public void Run()
    {
      this.NewGame();
      do
      {
        this.ProcessUserInput();
        this.DrawItems();
        this.ProcessAnswerStep();
        
        if (!this.IsGameFinished())
          this.DrawAnswer();
      }
      while (!this.IsGameFinished());
      this.DrawFinalMessage();
    }

    /// <summary>
    /// Инициализировать игру.
    /// </summary>
    private void NewGame()
    {
      Console.Clear();
      Console.ResetColor();

      this.itemsCount = initialItemsCount;
      this.lastUserInput = 0;

      this.DrawMainWindow();
      this.DrawItems();
      this.DrawUserInputWindow();
      this.DrawStartMessage();
    }

    /// <summary>
    /// Проверить, что игра завершилась.
    /// </summary>
    /// <returns>Истина, если игра завершилась.</returns>
    private bool IsGameFinished()
    {
            return this.itemsCount <= 1;
    }



    /// <summary>
    /// Отрисовать окно сообщений компьютера.
    /// </summary>
    private void DrawMessageWindow()
    {
      this.DrawWindow(dialogWindow, ConsoleColor.Black, ConsoleColor.White, Messages.DialogWindowTitle);
    }

    /// <summary>
    /// Отрисовать ответ компьютера.
    /// </summary>
    private void DrawAnswer()
    {
      this.DrawMessageWindow();
      this.PrintMessage(Messages.Question, this.dialogWindow, ConsoleColor.Green);
    }

    /// <summary>
    /// Отрисовать финальное сообщение.
    /// </summary>
    private void DrawFinalMessage()
    {
      this.DrawMessageWindow();
      this.PrintMessage(Messages.FinalMessage, dialogWindow, ConsoleColor.Green);
      Thread.Sleep(MillisecondsTimeoutBeforeComputerStep);
    }

    /// <summary>
    /// Напечатать ответ компьютера.
    /// </summary>
    /// <param name="message">Сообщение.</param>
    /// <param name="window">Границы окна, в которое необходимо напечатать сообщение.</param>
    /// <param name="fontColor">Цвет шрифта.</param>
    private void PrintMessage(string message, WindowRect window, ConsoleColor fontColor)
    {
      Console.CursorTop = ++window.Top;
      Console.ForegroundColor = fontColor;
      var cursorLeft = ++window.Left;
      var maxMessageLineLen = window.Width;

      var wordList = message.Split(' ');
      var messageLines = new List<string>();
      var newLine = "";
      foreach (var word in wordList)
      {
        var tempLine = newLine + ' ' + word;
        if (tempLine.Trim().Length > maxMessageLineLen)
        {
          messageLines.Add(newLine + '\n');
          tempLine = word;
        }
        newLine = tempLine.Trim();
      }
      messageLines.Add(newLine);

      var rnd = new Random();
      foreach (var line in messageLines)
      {
        Console.CursorLeft = cursorLeft;
        foreach (var c in line)
        {
          Thread.Sleep(rnd.Next(100) + 50);
          Console.Write(c);
        }
      }
      Console.ResetColor();
    }

    /// <summary>
    /// Отрисовать строку окна.
    /// </summary>
    /// <param name="indent">Отступ слева.</param>
    /// <param name="line">Строка окна.</param>
    private static void DrawWindowLine(int indent, string line)
    {
      Console.CursorLeft = indent;
      Console.Write(line);
    }

    /// <summary>
    /// Отрисовать окно.
    /// </summary>
    /// <param name="window">Окно.</param>
    /// <param name="backgroundColor">Цвет фона.</param>
    /// <param name="borderColor">Цвет границ.</param>
    /// <param name="title">Заголовок.</param>
    private void DrawWindow(WindowRect window, ConsoleColor backgroundColor, ConsoleColor borderColor, string title)
    {
      var leftTopCorner = '╔';
      var rightTopCorner = '╗';
      var leftBottomCorner = '╚';
      var rightBottomCorner = '╝';
      var vertical = '║';
      var horizontal = '═';

      var topLine = leftTopCorner + new string(horizontal, window.Width) + rightTopCorner + '\n';
      if (!string.IsNullOrEmpty(title))
      {
        title = ' ' + title + ' ';
        var topLinePartLen = (window.Width - title.Length) / 2;
        var topLimeMiddlePart = new string(horizontal, topLinePartLen) + title + new string(horizontal, topLinePartLen);
        while (topLimeMiddlePart.Length < window.Width)
          topLimeMiddlePart += horizontal;
        topLine = leftTopCorner + topLimeMiddlePart + rightTopCorner + '\n';
      }
      var horLine = vertical + new string(' ', window.Width) + vertical + '\n';
      var bottomLine = leftBottomCorner + new string(horizontal, window.Width) + rightBottomCorner;

      Console.ForegroundColor = borderColor;
      Console.BackgroundColor = backgroundColor;
      Console.CursorTop = window.Top;

      DrawWindowLine(window.Left, topLine);
      for (var i = 0; i < window.Height; i++)
        DrawWindowLine(window.Left, horLine);
      DrawWindowLine(window.Left, bottomLine);

      Console.SetCursorPosition(++window.Left, ++window.Top);
      Console.ResetColor();
    }

    /// <summary>
    /// Отрисовать главное окно.
    /// </summary>
    private void DrawMainWindow()
    {
      this.DrawWindow(this.mainWindow, ConsoleColor.Blue, ConsoleColor.Yellow, Messages.MainWindowTitle);
    }

    /// <summary>
    /// Отрисовать элементы.
    /// </summary>
    private void DrawItems()
    {
      this.DrawWindow(itemsWindow, ConsoleColor.DarkGray, ConsoleColor.White, string.Format(Messages.ItemsCount, this.itemsCount));

      var window = itemTemplateWindow;
      var maxItems = this.itemsCount;
      for (var i = 0; i < maxItems; i++)
      {
        this.DrawWindow(window, ConsoleColor.DarkBlue, ConsoleColor.Yellow, $"{i + 1}");
        window.Left += itemStep;
      }
    }

    /// <summary>
    /// Отрисовать окно пользовательского ввода.
    /// </summary>
    private void DrawUserInputWindow()
    {
      this.DrawWindow(this.userInputWindow, ConsoleColor.Black, ConsoleColor.White, Messages.UserChoiceCount);
    }

    /// <summary>
    /// Отрисовать приветствие.
    /// </summary>
    private void DrawStartMessage()
    {
      this.DrawMessageWindow();
      this.PrintMessage($"{Messages.StartMessage} {Messages.Question} {Messages.Help}", this.dialogWindow, ConsoleColor.Green);
    }

    /// <summary>
    /// Отрисовать выбор пользователя.
    /// </summary>
    private void DrawUserInput()
    {
      this.DrawUserInputWindow();

      var window = userInputWindow;
      window.Left += 2;
      window.Top += 2;
      window.Width = 2;
      window.Height -= 4;
      var maxItems = this.lastUserInput;
      for (var i = 0; i < maxItems; i++)
      {
        this.DrawWindow(window, ConsoleColor.Black, ConsoleColor.Green, null);
        window.Left += itemStep;
      }
    }

    /// <summary>
    /// Обработать ввод пользователя.
    /// </summary>
    private void ProcessUserInput()
    {
      var isInputFinished = false;
      do
      {
        var input = Console.ReadKey();
        
        switch (input.Key)
        {
          case ConsoleKey.UpArrow:
            if (this.lastUserInput < maxUserInput)
              this.lastUserInput++;
            break;

          case ConsoleKey.DownArrow:
            if (this.lastUserInput > 0)
              this.lastUserInput--;
            break;

          case ConsoleKey.Enter:
            isInputFinished = this.lastUserInput > 0;
            break;
        }
        this.DrawUserInput();
      }
      while (!isInputFinished);

      this.itemsCount = this.itemsCount - this.lastUserInput;
    }

    /// <summary>
    /// Обработать ответный шаг компьютера.
    /// </summary>
    private void ProcessAnswerStep()
    {
      // Данная стратегия не позволяет проиграть - компьютер всегда забирает последний элемент.
      // Чтобы можно было проиграть, нужно реализовать показ сообщения о проигрыше и заменить код выбора на закомментированный ниже.
      int computerItemsCount;

            computerItemsCount = 4- this.lastUserInput;
            //if (this.itemsCount % (maxUserInput + 1) == 1)
            //  computerItemsCount = new Random().Next(maxUserInput) + 1;
            //else
            //  if (this.itemsCount % (maxUserInput + 1) == 0)
            //  computerItemsCount = maxUserInput;
            //else
            //  computerItemsCount = this.itemsCount % (maxUserInput + 1) - 1;

            // Стратегия с вариантом проигрыша.
            //if (this.itemsCount > (2 * maxUserInput + 1))
            //  computerItemsCount = new Random().Next(maxUserInput) + 1;
            //else
            //  if (this.itemsCount <= maxUserInput + 1)
            //    computerItemsCount = this.itemsCount - 1;
            //  else
            //    computerItemsCount = this.itemsCount - maxUserInput - 2;

            var userItemsCount = this.lastUserInput;
      this.lastUserInput = 0;
      this.DrawUserInput();
      this.DrawMessageWindow();
      this.PrintMessage(string.Format(Messages.Answer, userItemsCount, computerItemsCount), this.dialogWindow, ConsoleColor.Green);
      this.itemsCount -= computerItemsCount;
      this.DrawItems();
      Thread.Sleep(MillisecondsTimeoutBeforeComputerStep);
    }
  }
}
