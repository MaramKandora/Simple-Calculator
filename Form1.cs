using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Media;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Calculator_windows_Forms_Project
{

    public partial class Form1 : Form
    {


        public Form1()
        {
            InitializeComponent();
        }
        struct strOpOprands
        {
            public double FirstOperand;
            public double SecondOperand;

        }


        enum enUnaryOpType
        {
            Reciprocal,
            Square,
            SquareRoot

        }

        struct strOperation
        {
            public bool IsTheLastOperationIsUnary;
            public char? OpSign;
            public bool IsOperationHasSign;
            public strOpOprands OpOperands;
        }


        strOperation _Operation;






        private void btnCalculSign_Click(object sender, EventArgs e)
        {
            Button SignButtton = (Button)sender;




            if (!_Operation.IsOperationHasSign)
            {
                if (lblCalculation.Text != "")
                {
                    _Operation.OpOperands.FirstOperand = Convert.ToDouble(lblCalculation.Text);
                    lblScreen.Text = _Operation.OpOperands.FirstOperand.ToString() + SignButtton.Tag.ToString();
                    lblCalculation.Text = "0";
                }
                else
                {
                    return;
                }
            }
            else
            {
                _Operation.OpOperands.SecondOperand = Convert.ToDouble(lblCalculation.Text);

                bool IsBothOperandsAreZeros = (_Operation.OpOperands.FirstOperand == 0) && (_Operation.OpOperands.SecondOperand == 0);

                if (IsBothOperandsAreZeros)
                {
                    for (int i = lblScreen.Text.Length - 1; i >= 0; i--)
                    {
                        if (isCalculaionSign(lblScreen.Text[i]))
                        {
                            lblScreen.Text = lblScreen.Text.Remove(i);
                            lblScreen.Text += SignButtton.Tag;
                            break;
                        }
                    }


                }
                else
                {
                    double CalculResult = GetResult();
                    lblScreen.Text = CalculResult.ToString() + SignButtton.Tag.ToString();
                    lblCalculation.Text = "0";
                    _Operation.OpOperands.FirstOperand = CalculResult;
                    _Operation.OpOperands.SecondOperand = 0;

                }
            }


            _Operation.OpSign = Convert.ToChar(SignButtton.Tag);
            _Operation.IsOperationHasSign = true;


        }

        private void btnNumber_Click(object sender, EventArgs e)
        {
            Button NumButton = (Button)sender;

            if (_Operation.IsTheLastOperationIsUnary)
            {
                lblCalculation.Text = "";
                _Operation.IsTheLastOperationIsUnary = false;
            }

            if (lblCalculation.Text == "0")
            {
                if (NumButton.Text == "0")
                    return;

                else
                    lblCalculation.Text = NumButton.Tag.ToString();
            }


            else
            {
                lblCalculation.Text += NumButton.Tag;
            }

        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(lblCalculation.Text))
                return;

            if (lblScreen.Text.Contains("="))
            {
                lblScreen.Text = "";
                _Operation.OpOperands.FirstOperand = 0;
                _Operation.OpOperands.SecondOperand = 0;
                _Operation.IsOperationHasSign = false;
                _Operation.OpSign = null;
                return;
            }

            lblCalculation.Text = lblCalculation.Text.Remove(lblCalculation.Text.Length - 1);

            if (lblCalculation.Text == "")
                lblCalculation.Text = "0";
        }

        void ZeroingCalculator()
        {
            lblCalculation.Text = "0";
            lblScreen.Text = "";
            _Operation.OpOperands.FirstOperand = 0;
            _Operation.OpOperands.SecondOperand = 0;
            _Operation.IsOperationHasSign = false;
            _Operation.OpSign = null;
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            ZeroingCalculator();
        }




        bool isCalculaionSign(char C)
        {
            char[] arrCalculationSings = { '+', '-', '*', '/' };

            for (int i = 0; i < arrCalculationSings.Length; i++)
            {
                if (arrCalculationSings[i] == C)
                {
                    return true;
                }
            }

            return false;
        }



        private void btnClearEntry_Click(object sender, EventArgs e)
        {
            if (lblScreen.Text.Contains('='))
            {
                lblScreen.Text = "";
                _Operation.OpOperands.FirstOperand = 0;
            }

            else if (lblScreen.Text != "")
            {
                if (!isCalculaionSign(lblScreen.Text[lblScreen.Text.Length - 1]))
                {
                    for (int i = lblScreen.Text.Length - 1; i >= 0; i--)
                    {
                        if (isCalculaionSign(lblScreen.Text[i]))
                        {
                            if (lblScreen.Text[i] == '-' && lblScreen.Text[i - 1] == 'E')
                                continue;

                            lblScreen.Text = lblScreen.Text.Remove(i + 1);

                            break;
                        }
                    }
                }
            }

            lblCalculation.Text = "0";
            _Operation.OpOperands.SecondOperand = 0;

        }

        private void btn_FlipSign_Click(object sender, EventArgs e)
        {


            if (string.IsNullOrWhiteSpace(lblCalculation.Text))
                return;

            if (lblCalculation.Text == "0")
                return;

            double Num = Convert.ToDouble(lblCalculation.Text);
            Num = Num * -1;

            lblCalculation.Text = Num.ToString();

        }


        double CalculatePercent(double Number, double Percent, char? OpSign)
        {
            if (OpSign == '+' || OpSign == '-')
            {
                return (Number * (Percent / 100));
            }

            else if (OpSign == '*' || OpSign == '/')
            {
                return (Percent / 100);
            }
            else
            {
                return (Percent / 100);
            }


        }

        private void btnPercent_Click(object sender, EventArgs e)
        {
            double Number = _Operation.OpOperands.FirstOperand;
            double Perccent = Convert.ToDouble(lblCalculation.Text);

            if (!_Operation.IsOperationHasSign)
            {
                if (Number == 0 || Perccent == 0)
                {
                    lblCalculation.Text = "0";
                    lblScreen.Text = "0";
                    return;
                }
            }

            double Result = CalculatePercent(Number, Perccent, _Operation.OpSign);

            _Operation.OpOperands.SecondOperand = Result;

            lblCalculation.Text = Result.ToString();


            lblScreen.Text = _Operation.OpOperands.FirstOperand.ToString() + _Operation.OpSign
                + _Operation.OpOperands.SecondOperand;



        }

        void PerformUnaryOperationOnEntry(enUnaryOpType OpType)
        {


            double Number = Convert.ToDouble(lblCalculation.Text);

            switch (OpType)
            {
                case enUnaryOpType.Reciprocal:
                    Number = 1 / Number;
                    break;

                case enUnaryOpType.Square:
                    Number = Number * Number;
                    break;

                case enUnaryOpType.SquareRoot:
                    Number = Convert.ToDouble(Math.Sqrt(Convert.ToDouble(Number)));
                    break;
            }

            if (_Operation.OpSign != null)
            {
                _Operation.OpOperands.SecondOperand = Number;
                lblScreen.Text = _Operation.OpOperands.FirstOperand.ToString() + _Operation.OpSign
                    + _Operation.OpOperands.SecondOperand.ToString();

            }
            else
            {
               
                lblScreen.Text = Number.ToString();
            }

            lblCalculation.Text = Number.ToString();

            _Operation.IsTheLastOperationIsUnary = true;

        }


        private void btn_1OverX_Click(object sender, EventArgs e)
        {
            PerformUnaryOperationOnEntry(enUnaryOpType.Reciprocal);

        }

        private void btn_SquareOfX_Click(object sender, EventArgs e)
        {
            PerformUnaryOperationOnEntry(enUnaryOpType.Square);

        }

        private void btn_SqrOfX_Click(object sender, EventArgs e)
        {
            PerformUnaryOperationOnEntry(enUnaryOpType.SquareRoot);
        }

        private void btnDot_Click(object sender, EventArgs e)
        {

            if (lblCalculation.Text.Contains('.'))
                return;


            else if (lblCalculation.Text == "")
                lblCalculation.Text = "0.";

            else
                lblCalculation.Text += '.';
        }

        private void Form1_Load(object sender, EventArgs e)
        {

            _Operation.IsOperationHasSign = false;
            _Operation.OpSign = null;
        }

        double GetResult()
        {


            double Result;
            switch (_Operation.OpSign)
            {
                case '+':
                    Result = (_Operation.OpOperands.FirstOperand) + (_Operation.OpOperands.SecondOperand);
                    break;
                case '-':
                    Result = (_Operation.OpOperands.FirstOperand) - (_Operation.OpOperands.SecondOperand);
                    break;
                case '/':
                    Result = (_Operation.OpOperands.FirstOperand) / (_Operation.OpOperands.SecondOperand);
                    break;
                case '*':
                    Result = (_Operation.OpOperands.FirstOperand) * (_Operation.OpOperands.SecondOperand);
                    break;
                default:
                    Result = (_Operation.OpOperands.FirstOperand) + (_Operation.OpOperands.SecondOperand);
                    break;

            }



            return Result;

        }


        private void btnEqual_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(lblScreen.Text) || string.IsNullOrWhiteSpace(lblCalculation.Text))
                return;

            if (_Operation.OpSign == null)
                return;

            if (lblScreen.Text.Contains('='))
            {
                _Operation.OpOperands.FirstOperand = Convert.ToDouble(lblCalculation.Text);

            }
            else
            {
                _Operation.OpOperands.SecondOperand = Convert.ToDouble(lblCalculation.Text);
            }


            lblScreen.Text = _Operation.OpOperands.FirstOperand.ToString() + _Operation.OpSign
                + _Operation.OpOperands.SecondOperand + '=';

            double CalculResult = GetResult();

            lblCalculation.Text = CalculResult.ToString();

            _Operation.IsOperationHasSign = false;
            _Operation.OpSign = null;


        }

      
    }
}
