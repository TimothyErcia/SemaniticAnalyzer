using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CompilerMachineProblem2
{
    public partial class Form1 : Form
    {
        string txt;
        //Lexical Analyzer
        class VarDeclareCheck
        {
            public bool HasDtype(string type)
            {
                if (type.Equals("int") || type.Equals("string") || type.Equals("double") || type.Equals("float") || type.Equals("bool") || type.Equals("char"))
                {
                    return true;
                }
                else
                { return false; }
            }

            public bool HasSemi(string type)
            {
                if(type.StartsWith("int") || type.StartsWith("string") || type.StartsWith("float") || type.StartsWith("double") || type.StartsWith("void") || type.Contains(" ") || type.StartsWith("="))
                { return false; }
                else if (type.EndsWith(";") || type.Equals(";"))
                {
                    return true;
                }
                else
                { return false; }
            }

            public bool HasExpression(string type)
            {
                if (type.Contains("="))
                { return true; }
                else
                { return false; }
            }

            public bool HasOperator(string type)
            {
                if (type.Equals("+") || type.Equals("-") || type.Equals("/") || type.Equals("*"))
                { return true; }
                else
                { return false; }
            }

            public bool HasTERM(string type)
            {
                if (type.Equals("int") || type.Equals("string") || type.Equals("double") || type.Equals("float") || type.Equals("char") ||
                    type.Equals("+") || type.Equals("-") || type.Equals("/") || type.Equals("*") || type.Equals("void") ||
                    type.Equals("main") || type.Equals("scanf") || type.Equals("printf") || type.Equals("=") || type.Equals("(") || type.Equals(")") ||
                    type.Equals(";") || type.StartsWith("\"") || type.EndsWith("\"") || type.EndsWith(";") || type.StartsWith("(") || type.StartsWith(")")
                    || type.StartsWith("{") || type.StartsWith("}") || type.Equals("{") || type.Equals("}"))
                {
                    return false;
                }
                else
                { return true; }
            }

            public bool HasKeyword(string type)
            {
                if(type.Equals("scanf") || type.Equals("printf") || type.Equals("main"))
                { return true; }
                else
                { return false; }
            }

            public bool HasStringLiteral(string type)
            {
                if(type.StartsWith("\"") || type.EndsWith("\"") || type.Contains("\""))
                { return true; }
                else
                { return false; }
            }
           
            public bool HasLbrace(string type)
            {
                if (type.StartsWith("{") || type.Equals("{"))
                { return true; }
                else
                { return false; }
            }

            public bool HasRbrace(string type)
            {
                if (type.StartsWith("}") || type.Equals("}"))
                { return true; }
                else
                { return false; }
            }

            public bool HasLscope(string type)
            {
                if(type.StartsWith("(") || type.Equals("("))
                { return true; }
                else
                { return false; }
            }

            public bool HasRscope(string type)
            {
                if (type.EndsWith(")") || type.Equals(")"))
                { return true; }
                else
                { return false; }
            }
        }

        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            //Initial Text
            txt = richTextBox1.Text;
            txt = txt.Replace(" ", "\n");
            txt = txt.Replace("(", "(\n");
            //txt = txt.Replace(")", "\n)");
            txt = txt.Replace(";", "\n;");
            txt = txt.Replace(",", "\n,");
            richTextBox4.Text = txt;
            string[] str = richTextBox4.Lines;
            LinkedList<string> getList = new LinkedList<string>();
            List<string> errorList = new List<string>();
            VarDeclareCheck check = new VarDeclareCheck();
            int j = 2;
            int i = 0;
            int cBrace = 0;

            //Algorithm
            try
            {
                do
                {
                    //Basic Variable
                    if (check.HasDtype(str[i]))
                    {
                        getList.AddLast(str[i]);
                        //If it has Identifier
                        if(check.HasTERM(str[i + 1]))
                        {
                            getList.AddLast(str[i + 1]);
                            //if it has a Semicolon
                            if(check.HasSemi(str[i + 2]))
                            {
                                getList.AddLast(str[i + 2]);
                                i = i + 3;
                            }
                            //Basic var declare with expression or with TERM
                            else if (check.HasExpression(str[i + 2]))
                            {
                                getList.AddLast(str[i + 2]);
                                //if it has double quote in the beggining or in the end
                                if (check.HasStringLiteral(str[i + 3]) || check.HasTERM(str[i + 3]))
                                {
                                    getList.AddLast(str[i + 3]);
                                    //if it has semicolon
                                    j = j + 2;
                                    RECURSEVAR:
                                    if(check.HasSemi(str[i + j]))
                                    {
                                        getList.AddLast(str[i + j]);
                                        j = j + 1;
                                        i = i + j;
                                    }

                                    //if it has operator
                                    else if (check.HasOperator(str[i + j]))
                                    {
                                        getList.AddLast(str[i + j]);
                                        j = j + 1;
                                        //if it has TERM or double quote
                                        if (check.HasTERM(str[i + j]) || check.HasStringLiteral(str[i + j]))
                                        {
                                            getList.AddLast(str[i + j]);
                                            //if it has Semicolon
                                            j = j + 1;
                                            if(check.HasSemi(str[i + j]))
                                            {
                                                getList.AddLast(str[i + j]);
                                                i = i + j;
                                            }
                                            else
                                            {
                                                errorList.Add(str[i + j]);
                                                richTextBox3.AppendText("\n Syntax Error: Missing ");
                                                break;
                                            }
                                        }
                                        else
                                        {
                                            errorList.Add(str[i + j]);
                                            richTextBox3.AppendText("\n Syntax Error: Missing ");
                                            break;
                                        }
                                    }
                                    //recursion of string inside of declaration
                                    else if(check.HasTERM(str[i + j]) || check.HasStringLiteral(str[i + j]))
                                    {
                                        getList.AddLast(str[i + j]);
                                        j = j + 1;
                                        goto RECURSEVAR;
                                    }

                                    else
                                    { errorList.Add(str[i + 4]);
                                        richTextBox3.AppendText("\n Syntax Error ");
                                        break; }
                                }

                                else
                                { errorList.Add(str[i + 3]);
                                    richTextBox3.AppendText("\n Syntax Error");
                                    break; }
                            }
                            else
                            { errorList.Add(str[i + 2]);
                                richTextBox3.AppendText("\n Missing Expression");
                                break; }
                        }
                        else
                        { errorList.Add(str[i + 1]);
                            richTextBox3.AppendText("\n Missing identifier");
                            break; }
                    }

                    //printf & scanf
                    else if(check.HasKeyword(str[i]))
                    {
                        getList.AddLast(str[i]);
                        //if it has ( parenthesis
                        if (check.HasLscope(str[i + 1]))
                        {
                            getList.AddLast(str[i + 1]); 
                             //if it has double quote or ) parenthesis *recursion may occur
                                ENDSRING:
                                if(check.HasStringLiteral(str[i + j]))
                                {
                                    getList.AddLast(str[i + j]);
                                    j = j + 1;
                                    //if it has ) parentheisi
                                    if (check.HasRscope(str[i + j]) || check.HasStringLiteral(str[i + j]))
                                    {
                                        getList.AddLast(str[i + j]);
                                        j = j + 1;
                                        //if it has Semicolon
                                        if (check.HasSemi(str[i + j]))
                                        {
                                            getList.AddLast(str[i + j]);
                                        j = j + 1;
                                            i = i + j;
                                        }
                                        else
                                        {
                                            errorList.Add(str[i + j]);
                                            richTextBox3.AppendText("\n Syntax Error ");
                                            break;
                                        }
                                    }
                                    else
                                    {
                                        errorList.Add(str[i + j]);
                                        richTextBox3.AppendText("\n Syntax Error ");
                                        break;
                                    }
                                }
                                //or if it has TERM
                                else if(check.HasTERM(str[i + j]))
                                {
                                    getList.AddLast(str[i + j]);
                                    j = j + 1;
                                    goto ENDSRING;
                                }

                                else if(check.HasRscope(str[i + j]))
                                {
                                    getList.AddLast(str[i + j]);
                                    j = j + 1;
                                    if(check.HasSemi(str[i + j]))
                                    {
                                        getList.AddLast(str[i + j]);
                                        j = j + 1;
                                        i = i + j;
                                    }
                                    else
                                    {
                                        errorList.Add(str[i + j]);
                                        richTextBox3.AppendText("\n Syntax Error ");
                                        break;
                                    }
                                }

                                else
                                { errorList.Add(str[i + j]);
                                    richTextBox3.AppendText("\n Syntax Error ");
                                    break; }
                            }
                            //or if it has ) parenthesis
                            else if(check.HasRscope(str[i + j]))
                            {
                                getList.AddLast(str[i + j]);
                                if (check.HasSemi(str[i + j]))
                                {
                                    getList.AddLast(str[i + j]);
                                    i = i + 5;
                                }
                                else
                                {
                                    errorList.Add(str[i + j]);
                                    richTextBox3.AppendText("\n Syntax Error ");
                                    break;
                                }
                            }
                            else
                            { errorList.Add(str[i + j]);
                                richTextBox3.AppendText("\n Syntax Error ");
                                break; }
                        }

                    //Function Declare (void)
                    else if (str[i] == "void")
                    {
                        getList.AddLast(str[i]);
                        //if function has Keyword or TERM
                        if (check.HasKeyword(str[i + 1]) || check.HasTERM(str[i + 1]))
                        {
                            getList.AddLast(str[i + 1]);
                            //if function has ( parenthesis
                            if (check.HasLscope(str[i + 2]))
                            {
                                getList.AddLast(str[i + 2]);
                                //if function has ) parentheis 
                                if (check.HasRscope(str[i + 3]))
                                {
                                    getList.AddLast(str[i + 3]); 
                                    //if function has opening brace
                                    if(check.HasLbrace(str[i + 4]))
                                    {
                                        cBrace++;
                                        getList.AddLast(str[i + 4]); 
                                        i = i + 5;
                                    }
                                    else
                                    {
                                        richTextBox3.AppendText("\n Missing { brace");
                                        break;
                                    }
                                }
                                else
                                {
                                    errorList.Add(str[i + 3]);
                                    richTextBox3.AppendText("\n Syntax Error: Missing )");
                                    break;
                                }
                            }
                            else
                            { errorList.Add(str[i + 2]);
                                richTextBox3.AppendText("\n Missing (");
                                break; }
                        }
                        else
                        { errorList.Add(str[i + 1]);
                            richTextBox3.AppendText("\n Missing keyword");
                            break; }
                    }

                    //if function has opening brace
                    else if (check.HasRbrace(str[i]))
                    {
                        cBrace++;
                        if (cBrace % 2 == 0)
                        {
                            getList.AddLast(str[i]);
                            i = i + 2;
                        }
                        else
                        {
                            richTextBox3.AppendText("\n Missing { brace");
                            break;
                        }
                    }

                    else
                    {
                        richTextBox3.AppendText("\n Missing Datatype");
                        break;
                    }

                } while (i <= str.Length);
            }
            catch
            { }
                
            foreach(var item in getList)
            {
                richTextBox2.AppendText(item + " ");
            }

            foreach (var item in errorList)
            {
                richTextBox3.AppendText(item + " ");
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            richTextBox2.Text = "";
            richTextBox3.Text = "";
            richTextBox4.Text = "";
        }
    }
}
