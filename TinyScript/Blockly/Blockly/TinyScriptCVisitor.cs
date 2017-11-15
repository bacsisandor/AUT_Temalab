using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Antlr4.Runtime.Misc;
using Antlr4.Runtime.Tree;

namespace Blockly
{
    public class ExpressionString
    {
        public string String { get; private set; }
        public VariableType Type { get; private set; }

        public ExpressionString(string str, VariableType type)
        {
            String = str;
            Type = type;
        }

        public static implicit operator ExpressionString(string str)
        {
            return new ExpressionString(str, VariableType.VOID);
        }

        public override string ToString()
        {
            return String;
        }
    }

    public class TinyScriptCVisitor : TinyScriptBaseVisitor<ExpressionString>
    {
        public override ExpressionString VisitProgram([NotNull] TinyScriptParser.ProgramContext context)
        {
            string varDecl = VisitVariableDeclarationList(context.variableDeclarationList()).ToString();
            string statements = VisitStatementList(context.statementList()).ToString();
            return varDecl + "\n\n" + statements;
        }

        public override ExpressionString VisitVariableDeclarationList([NotNull] TinyScriptParser.VariableDeclarationListContext context)
        {
            return VisitList(context.variableDeclaration());
        }

        public override ExpressionString VisitStatementList([NotNull] TinyScriptParser.StatementListContext context)
        {
            return VisitList(context.statement());
        }

        private ExpressionString VisitList(IParseTree[] tree)
        {
            if (tree.Length == 0)
            {
                return "";
            }
            string result = "";
            foreach (IParseTree subtree in tree)
            {
                result += Visit(subtree) + "\n";
            }
            return result;
        }

        public override ExpressionString VisitStatement([NotNull] TinyScriptParser.StatementContext context)
        {
            return Visit(context.GetChild(0));
        }

        public override ExpressionString VisitVariableDeclaration([NotNull] TinyScriptParser.VariableDeclarationContext context)
        {
            return Visit(context.GetChild(0));
        }

        public override ExpressionString VisitVariableDeclaration1([NotNull] TinyScriptParser.VariableDeclaration1Context context)
        {
            string typeName = context.typeName().GetText();
            string varName = context.varName().GetText();
            if (context.expression() == null)
            {
                return $"{ typeName } { varName };";
            }
            ExpressionString expression = VisitExpression(context.expression());
            return $"{ typeName } { varName } = { expression };";
        }

        public override ExpressionString VisitVariableDeclaration2([NotNull] TinyScriptParser.VariableDeclaration2Context context)
        {
            string varName = context.varName().GetText();
            ExpressionString expression = VisitExpression(context.expression());
            return $"{ expression.Type } { varName } = { expression };";
        }

        public override ExpressionString VisitExpression([NotNull] TinyScriptParser.ExpressionContext context)
        {
            if (context.compareOp() == null)
            {
                return VisitSum(context.sum()[0]);
            }
            ExpressionString leftExpr = (ExpressionString)VisitSum(context.sum()[0]);
            ExpressionString rightExpr = (ExpressionString)VisitSum(context.sum()[1]);
            return new ExpressionString($"{ leftExpr } { context.compareOp().GetText() } { rightExpr }", VariableType.BOOLEAN);
        }

        public override ExpressionString VisitSum([NotNull] TinyScriptParser.SumContext context)
        {
            string result = "";
            var products = context.product();
            var ops = context.PLUSMINUS();
            result += VisitProduct(products[0]);
            for (int i = 0; i < ops.Length; i++)
            {
                result += $" { ops[i].GetText() } { VisitProduct(products[i + 1]) }";
            }
            return new ExpressionString(result, VariableType.INT);
        }

        public override ExpressionString VisitProduct([NotNull] TinyScriptParser.ProductContext context)
        {
            string result = "";
            var args = context.signedArgument();
            var ops = context.MULDIV();
            result += VisitSignedArgument(args[0]);
            for (int i = 0; i < ops.Length; i++)
            {
                result += $" { ops[i].GetText() } { VisitSignedArgument(args[i + 1]) }";
            }
            return new ExpressionString(result, VariableType.INT);
        }

        public override ExpressionString VisitSignedArgument([NotNull] TinyScriptParser.SignedArgumentContext context)
        {
            if (context.PLUSMINUS() != null)
            {
                return new ExpressionString($"{ context.PLUSMINUS().GetText() }{ VisitArgument(context.argument()) }", VariableType.INT);
            }
            return VisitArgument(context.argument());
        }

        public override ExpressionString VisitArgument([NotNull] TinyScriptParser.ArgumentContext context)
        {
            if (context.expression() != null)
            {
                ExpressionString expr = VisitExpression(context.expression());
                return new ExpressionString($"({ expr })", expr.Type);
            }
            if (context.indexedArray() != null)
            {
                return VisitIndexedArray(context.indexedArray());
            }
            if (context.functionCall() != null)
            {
                return VisitFunctionCall(context.functionCall());
            }
            if (context.varName() != null)
            {
                return context.varName().GetText();
            }
            return VisitValue(context.value());
        }

        public override ExpressionString VisitValue([NotNull] TinyScriptParser.ValueContext context)
        {
            if (context.INT() != null)
            {
                return new ExpressionString(context.GetText(), VariableType.INT);
            }
            if (context.BOOLEAN() != null)
            {
                return new ExpressionString(context.GetText(), VariableType.BOOLEAN);
            }
            if (context.STRING() != null)
            {
                return new ExpressionString(context.GetText(), VariableType.STRING);
            }
            return new ExpressionString("null", VariableType.NULL);
        }

        public override ExpressionString VisitIfStatement([NotNull] TinyScriptParser.IfStatementContext context)
        {
            string result = $"if ({ VisitExpression(context.expression()) }) ";
            result += VisitBlock(context.block());
            foreach (var elseIf in context.elseIfStatement())
            {
                result += "\n" + VisitElseIfStatement(elseIf);
            }
            if (context.elseStatement() != null)
            {
                result += "\n" + VisitElseStatement(context.elseStatement());
            }
            return result;
        }

        public override ExpressionString VisitElseIfStatement([NotNull] TinyScriptParser.ElseIfStatementContext context)
        {
            string result = $"else if ({ VisitExpression(context.expression()) }) ";
            result += VisitBlock(context.block());
            return result;
        }

        public override ExpressionString VisitElseStatement([NotNull] TinyScriptParser.ElseStatementContext context)
        {
            string result = $"else ";
            result += VisitBlock(context.block());
            return result;
        }

        public override ExpressionString VisitBlock(TinyScriptParser.BlockContext context)
        {
            return $"{{\n{ VisitStatementList(context.statementList()) }\n}}";
        }

        public override ExpressionString VisitWhileStatement([NotNull] TinyScriptParser.WhileStatementContext context)
        {
            string result = $"while ({ VisitExpression(context.expression()) }) ";
            result += VisitBlock(context.block());
            return result;
        }

        public override ExpressionString VisitDoWhileStatement([NotNull] TinyScriptParser.DoWhileStatementContext context)
        {
            string result = "do ";
            result += VisitBlock(context.block());
            result += $"\nwhile ({ VisitExpression(context.expression()) });";
            return result;
        }

        public override ExpressionString VisitForStatement([NotNull] TinyScriptParser.ForStatementContext context)
        {
            string varName = context.varName().GetText();
            string assignExpr = VisitExpression(context.expression()[0]).ToString();
            string cond = VisitExpression(context.expression()[1]).ToString();
            string incr = "";
            if (context.incrementation() != null)
            {
                incr = VisitIncrementation(context.incrementation()).ToString();
            }
            else
            {
                incr = VisitDecrementation(context.decrementation()).ToString();
            }
            string result = $"for ({ varName } = { assignExpr }; { cond }; { incr }) ";
            result += VisitBlock(context.block());
            return result;
        }

        public override ExpressionString VisitIncrementation([NotNull] TinyScriptParser.IncrementationContext context)
        {
            string varName = context.varName().GetText();
            if (context.expression() == null)
            {
                return $"{ varName }++";
            }
            string expr = VisitExpression(context.expression()).ToString();
            return $"{ varName } += { expr }";
        }

        public override ExpressionString VisitDecrementation([NotNull] TinyScriptParser.DecrementationContext context)
        {
            string varName = context.varName().GetText();
            if (context.expression() == null)
            {
                return $"{ varName }--";
            }
            string expr = VisitExpression(context.expression()).ToString();
            return $"{ varName } -= { expr }";
        }

        public override ExpressionString VisitIncrementStatement([NotNull] TinyScriptParser.IncrementStatementContext context)
        {
            return VisitIncrementation(context.incrementation()) + ";";
        }

        public override ExpressionString VisitDecrementStatement([NotNull] TinyScriptParser.DecrementStatementContext context)
        {
            return VisitDecrementation(context.decrementation()) + ";";
        }

        public override ExpressionString VisitAssignmentStatement([NotNull] TinyScriptParser.AssignmentStatementContext context)
        {
            string varName = context.varName().GetText();
            string expr = VisitExpression(context.expression()).ToString();
            return $"{ varName } = { expr };";
        }

        public override ExpressionString VisitFunctionCallStatement([NotNull] TinyScriptParser.FunctionCallStatementContext context)
        {
            return VisitFunctionCall(context.functionCall()) + ";";
        }

        public override ExpressionString VisitFunctionCall([NotNull] TinyScriptParser.FunctionCallContext context)
        {
            string name = context.functionName().GetText();
            string result = $"{ name }(";
            for (int i = 0; i < context.expression().Length; i++)
            {
                if (i > 0)
                {
                    result += ", ";
                }
                result += VisitExpression(context.expression()[i]);
            }
            result += ")";
            return result;
        }

        public override ExpressionString VisitArrayDeclaration([NotNull] TinyScriptParser.ArrayDeclarationContext context)
        {
            string typeName = context.typeName().GetText();
            string varName = context.varName().GetText();
            string expr = VisitExpression(context.expression()).ToString();
            return $"{ typeName } { varName }[{ expr }];";
        }

        public override ExpressionString VisitArrayAssignmentStatement([NotNull] TinyScriptParser.ArrayAssignmentStatementContext context)
        {
            string varName = context.varName().GetText();
            string expr1 = VisitExpression(context.expression()[0]).ToString();
            string expr2 = VisitExpression(context.expression()[1]).ToString();
            return $"{ varName }[{ expr1 }] = { expr2 };";
        }

        public override ExpressionString VisitIndexedArray([NotNull] TinyScriptParser.IndexedArrayContext context)
        {
            string varName = context.varName().GetText();
            string expr = VisitExpression(context.expression()).ToString();
            return $"{ varName }[{ expr }]";
        }

        public override ExpressionString VisitArrayInitialization([NotNull] TinyScriptParser.ArrayInitializationContext context)
        {
            string typeName = context.typeName().GetText();
            string varName = context.varName().GetText();
            string result = $"{ typeName } { varName }[] = {{ ";
            for (int i = 0; i < context.expression().Length; i++)
            {
                if (i > 0)
                {
                    result += ", ";
                }
                result += VisitExpression(context.expression()[i]);
            }
            result += " };";
            return result;
        }

        public override ExpressionString VisitReadStatement([NotNull] TinyScriptParser.ReadStatementContext context)
        {
            return $"read({ context.varName().GetText() });";
        }
    }
}
