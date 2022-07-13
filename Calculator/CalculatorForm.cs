using Calculator.Stack;
using System;
using System.Collections.Generic;
using System.Windows.Forms;


//Разработать программу, имитирующую работу калькулятора. 
//    Ввод выражения осуществляется в инфиксной форме. 
//    Для вычисления значения выражения необходимо использовать стек. 
//    Арифметическое выражение может содержать в себе целые и дробные числа, знаки математических операций, 
//    круглые скобки, переменные (не менее трех), деление по модулю, функции (возведение в степень, 
//    логарифм по основанию два). 
//    Предусмотреть проверку корректности введенного выражения.

namespace Calculator
{
    public partial class CalculatorForm : Form
    {
        public CalculatorForm()
        {
            InitializeComponent();
            InitializeButtons();
        }

        private void InitializeButtons()
        {
            btn0.Click += (s, e) => WriteOutput("0");
            btn1.Click += (s, e) => WriteOutput("1");
            btn2.Click += (s, e) => WriteOutput("2");
            btn3.Click += (s, e) => WriteOutput("3");
            btn4.Click += (s, e) => WriteOutput("4");
            btn5.Click += (s, e) => WriteOutput("5");
            btn6.Click += (s, e) => WriteOutput("6");
            btn7.Click += (s, e) => WriteOutput("7");
            btn8.Click += (s, e) => WriteOutput("8");
            btn9.Click += (s, e) => WriteOutput("9");

            btnPlus.Click += (s, e) => WriteOutput("+");
            btnMinus.Click += (s, e) => WriteOutput("-");
            btnDevide.Click += (s, e) => WriteOutput("/");
            btnMulti.Click += (s, e) => WriteOutput("*");

            btnLeftBracket.Click += (s, e) => WriteOutput("(");
            btnRightBracket.Click += (s, e) => WriteOutput(")");

            btnEqual.Click += (s, e) => Calculate();
        }

        private void WriteOutput(string str) => tbOutput.Text = tbOutput.Text + str;

        private void Calculate()
        {
            tbRpn.Text = GetExpression(tbOutput.Text);
            MessageBox.Show(
                $"Результат вычисления: {Counting(GetExpression(tbOutput.Text))}", 
                "Результат", 
                MessageBoxButtons.OK, 
                MessageBoxIcon.Information);

        }

        //Преобразование в обратную польскую нотацию
        private string GetExpression(string input)
        {
            string output = string.Empty;
            MyStack<char> operStack = new MyStack<char>();

            for (int i = 0; i < input.Length; i++)
            {
                if (IsDelimeter(input[i]))
                    continue;

                //Если символ - цифра
                if (Char.IsDigit(input[i])) 
                {
                    //Читаем до разделителя или оператора, что бы получить число
                    while (!IsDelimeter(input[i]) && !IsOperator(input[i]))
                    {
                        output += input[i];
                        i++;
                        if (i == input.Length) break;
                    }

                    output += " ";
                    i--;
                }

                //Если символ - оператор
                if (IsOperator(input[i]))
                {
                    if (input[i] == '(') 
                        operStack.Push(input[i]); 
                    else if (input[i] == ')')
                    {
                        //Выписываем все операторы до открывающей скобки в строку
                        char s = operStack.Pop();

                        while (s != '(')
                        {
                            output += s.ToString() + ' ';
                            s = operStack.Pop();
                        }
                    }
                    else
                    {
                        if (operStack.Count > 0)
                            if (GetPriority(input[i]) <= GetPriority(operStack.Peek()))
                                output += operStack.Pop().ToString() + " ";

                        operStack.Push(char.Parse(input[i].ToString()));
                    }
                }
            }

            //Извлекаем всё из стека в строку
            while (operStack.Count > 0)
                output += operStack.Pop() + " ";

            return output;
        }

        private double Counting(string input)
        {
            double result = 0;
            MyStack<double> temp = new MyStack<double>();

            for (int i = 0; i < input.Length; i++)
            {
                if (Char.IsDigit(input[i]))
                {
                    string a = string.Empty;

                    while (!IsDelimeter(input[i]) && !IsOperator(input[i]))
                    {
                        a += input[i];
                        i++;
                        if (i == input.Length) break;
                    }
                    temp.Push(double.Parse(a));
                    i--;
                }
                else if (IsOperator(input[i]))
                {
                    //Берем два последних значения из стека
                    double a = temp.Pop();
                    double b = temp.Pop();

                    //Производим операцию
                    switch (input[i]) 
                    {
                        case '+': result = b + a; break;
                        case '-': result = b - a; break;
                        case '*': result = b * a; break;
                        case '/': result = b / a; break;
                        case '^': result = double.Parse(Math.Pow(double.Parse(b.ToString()), double.Parse(a.ToString())).ToString()); break;
                    }
                    //Результат вычисления записываем обратно в стек
                    temp.Push(result); 
                }
            }
            return temp.Peek();
        }

        //Проверка на принадлежность разделителю
        private bool IsDelimeter(char c)
        {
            if ((" =".IndexOf(c) != -1))
                return true;
            return false;
        }

        //Проверка на принадлежность оператору
        private bool IsOperator(char с)
        {
            if (("+-/*^()".IndexOf(с) != -1))
                return true;
            return false;
        }

        //Получение приоритета
        private byte GetPriority(char s)
        {
            switch (s)
            {
                case '(': return 0;
                case ')': return 1;
                case '+': return 2;
                case '-': return 3;
                case '*': return 4;
                case '/': return 4;
                case '^': return 5;
                default: return 6;
            }
        }
    }    
}
