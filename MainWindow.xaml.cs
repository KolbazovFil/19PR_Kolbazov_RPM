using System.Windows;
using System.Windows.Controls;
namespace _19PR_Kolbazov_RPM
{
    public partial class MainWindow : Window
    {
        private const int Size = 9; // Константа для сети 9х9.
        private TextBox[,] textBoxes = new TextBox[Size, Size]; // Двумерный массив, который будет хранить ссылки на элементы TextBox.
        public MainWindow()
        {
            InitializeComponent();
            InitializeSudokuGrid();
            GenerateSudoku();
        }
        private void InitializeSudokuGrid() // Отвечает за инициализацию сетки, связывая элементы интерфейса (TextBox) с массивом textBoxes.
        {
            int index = 0;  // Для отслеживания текущей позиции в сетке.
            foreach (var child in SudokuGrid.Children)  // Перебирает все дочерние элементы в контейнере SudokuGrid
            {
                if (child is TextBox textBox)   // Обрабатывать только TextBox.
                {
                    textBoxes[index / Size, index % Size] = textBox;    // Присваиваем текущий TextBox соответствующей позиции в двумерном массиве.
                    index++;    // Увеличивает значение переменной index на 1.
                }
            }
        }
        private void TextBox_TextChanged(object sender, TextChangedEventArgs e) // Срабатывает каждый раз, когда текст в любом TextBox изменяется.
        {
            TextBox textBox = sender as TextBox;    // ссылается на элемент, вызвавший событие. 
            if (textBox != null && !int.TryParse(textBox.Text, out _))  // Проверяем, что textBox не равен null и что текст в нем не является числом.
            {
                textBox.Text = string.Empty; // Очистка, если введено не число
            }
        }
        private void GenerateSudoku()   // Начальная конфигурация судоку
        {
            // Двумерный массив
            int[,] mass = {
                {0, 0, 2, 0, 8, 0, 0, 6, 0},
                {0, 5, 6, 9, 1, 7, 0, 3, 0},
                {0, 4, 0, 0, 5, 0, 8, 7, 1},
                {0, 9, 0, 0, 0, 0, 6, 0, 0},
                {6, 7, 1, 0, 9, 5, 2, 0, 0},
                {0, 0, 0, 0, 2, 0, 1, 0, 0},
                {1, 6, 7, 0, 3, 0, 5, 9, 0},
                {4, 8, 0, 0, 7, 0, 3, 0, 0},
                {0, 2, 5, 4, 6, 0, 0, 0, 0}
            };
            for (int row = 0; row < Size; row++)
            {
                for (int col = 0; col < Size; col++)
                {
                    if (mass[row, col] != 0)
                    {
                        textBoxes[row, col].Text = mass[row, col].ToString();
                        textBoxes[row, col].IsReadOnly = true; // Заблокировать редактирование
                    }
                    else
                    {
                        textBoxes[row, col].IsReadOnly = false; // Разрешить редактирование
                    }
                }
            }
        }
        private void NewGame_Click(object sender, RoutedEventArgs e)    // Вызывается при нажатии кнопки новой игры
        {
            ClearSudoku();
            GenerateSudoku();
        }
        private void ClearSudoku()  // Очистка 
        {
            for (int row = 0; row < Size; row++)
            {
                for (int col = 0; col < Size; col++)
                {
                    textBoxes[row, col].Text = string.Empty; // Очищаем текстовые поля
                    textBoxes[row, col].IsReadOnly = false; // Разрешаем редактирование
                }
            }
        }
        private void Check_Click(object sender, RoutedEventArgs e)  // Сообщение о результате
        {
            if (CheckSudoku())
            {
                MessageBox.Show("Судоку решено правильно!", "Результат", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else
            {
                MessageBox.Show("Попробуйте еще раз!", "Результат", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }
        private bool CheckSudoku()  // Проверка правильности заполнения.
        {
            // Проверка на дубликаты
            for (int row = 0; row < Size; row++)
            {
                for (int col = 0; col < Size; col++)
                {
                    if (!string.IsNullOrEmpty(textBoxes[row, col].Text))
                    {
                        int value;
                        if (!int.TryParse(textBoxes[row, col].Text, out value) || value < 1 || value > 9)
                        {
                            return false; // Если значение не корректное
                        }

                        // Проверка на дубликаты
                        if (!IsValid(value, row, col))
                        {
                            return false;
                        }
                    }
                }
            }
            return true;
        }
        private bool IsValid(int value, int row, int col)   // проверка, является ли введенное значение допустимым в строке, столбце и квадрате 3x3.
        {
            // Проверка строки
            for (int c = 0; c < Size; c++)
            {
                if (c != col && textBoxes[row, c].Text == value.ToString())
                {
                    return false; // Дубликат в строке
                }
            }

            // Проверка столбца
            for (int r = 0; r < Size; r++)
            {
                if (r != row && textBoxes[r, col].Text == value.ToString())
                {
                    return false; // Дубликат в столбце
                }
            }
            // Проверка квадрата 3x3
            int boxRowStart = (row / 3) * 3;
            int boxColStart = (col / 3) * 3;
            for (int r = boxRowStart; r < boxRowStart + 3; r++)
            {
                for (int c = boxColStart; c < boxColStart + 3; c++)
                {
                    if ((r != row || c != col) && textBoxes[r, c].Text == value.ToString())
                    {
                        return false; // Дубликат в квадрате 3x3
                    }
                }
            }
            return true;
        }
    }
}