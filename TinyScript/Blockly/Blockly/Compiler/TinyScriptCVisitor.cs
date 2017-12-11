using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Antlr4.Runtime.Misc;
using Antlr4.Runtime.Tree;
using Antlr4.Runtime;

namespace Blockly
{
    public class TinyScriptCVisitor : TinyScriptBaseVisitor<string>
    {
        private TinyScriptVisitor.ITypeData typeData;

        public TinyScriptCVisitor(TinyScriptVisitor.ITypeData typeData)
        {
            this.typeData = typeData;
        }

        public override string VisitProgram([NotNull] TinyScriptParser.ProgramContext context)
        {
            string fnDef = VisitFunctionDefinitionList(context.functionDefinitionList());
            string varDecl = VisitVariableDeclarationList(context.variableDeclarationList());
            string statements = VisitStatementList(context.statementList());
            string block = $"{ varDecl }\n\n{ statements }\nreturn 0;";
            Indent(ref block);
            string result = $"#include \"TinyScript.h\"\n\n{ fnDef }\n\nint main()\n{{\n{ block }\n}}";
            return result.Replace("\n", Environment.NewLine);
        }

        public override string VisitVariableDeclarationList([NotNull] TinyScriptParser.VariableDeclarationListContext context)
        {
            return VisitList(context.variableDeclaration(), "\n");
        }

        public override string VisitStatementList([NotNull] TinyScriptParser.StatementListContext context)
        {
            return VisitList(context.statement(), "\n");
        }

        public override string VisitFunctionDefinitionList([NotNull] TinyScriptParser.FunctionDefinitionListContext context)
        {
            return VisitList(context.functionDefinition(), "\n\n");
        }

        private string VisitList(IParseTree[] tree, string separator)
        {
            string result = "";
            for (int i = 0; i < tree.Length; i++)
            {
                if (i > 0)
                {
                    result += separator;
                }
                result += Visit(tree[i]);
            }
            return result;
        }

        public override string VisitStatement([NotNull] TinyScriptParser.StatementContext context)
        {
            return Visit(context.GetChild(0));
        }

        public override string VisitVariableDeclaration([NotNull] TinyScriptParser.VariableDeclarationContext context)
        {
            return Visit(context.GetChild(0));
        }

        public override string VisitVariableDeclaration1([NotNull] TinyScriptParser.VariableDeclaration1Context context)
        {
            return VariableDeclaration(context.varName(), context.expression());
        }

        public override string VisitVariableDeclaration2([NotNull] TinyScriptParser.VariableDeclaration2Context context)
        {
            return VariableDeclaration(context.varName(), context.expression());
        }

        private string VariableDeclaration(TinyScriptParser.VarNameContext varContext, TinyScriptParser.ExpressionContext exprContext)
        {
            string varName = varContext.GetText();
            VariableType type = typeData.GetVariableType(varName);
            if (exprContext == null)
            {
                return $"{ type.ToCPPTypeName() } { varName };";
            }
            string expression = VisitExpression(exprContext);
            return $"{ type.ToCPPTypeName() } { varName } = { expression };";
        }

        public override string VisitExpression([NotNull] TinyScriptParser.ExpressionContext context)
        {
            if (context.compareOp() == null)
            {
                return VisitSum(context.sum()[0]);
            }
            string leftExpr = VisitSum(context.sum()[0]);
            string rightExpr = VisitSum(context.sum()[1]);
            return $"{ leftExpr } { context.compareOp().GetText() } { rightExpr }";
        }

        public override string VisitSum([NotNull] TinyScriptParser.SumContext context)
        {
            string result = "";
            var products = context.product();
            var ops = context.PLUSMINUS();
            result += VisitProduct(products[0]);
            for (int i = 0; i < ops.Length; i++)
            {
                result += $" { ops[i].GetText() } { VisitProduct(products[i + 1]) }";
            }
            return result;
        }

        public override string VisitProduct([NotNull] TinyScriptParser.ProductContext context)
        {
            string result = "";
            var args = context.signedArgument();
            var ops = context.MULDIV();
            result += VisitSignedArgument(args[0]);
            for (int i = 0; i < ops.Length; i++)
            {
                result += $" { ops[i].GetText() } { VisitSignedArgument(args[i + 1]) }";
            }
            return result;
        }

        public override string VisitSignedArgument([NotNull] TinyScriptParser.SignedArgumentContext context)
        {
            if (context.PLUSMINUS() != null)
            {
                return $"{ context.PLUSMINUS().GetText() }{ VisitArgument(context.argument()) }";
            }
            return VisitArgument(context.argument());
        }

        public override string VisitArgument([NotNull] TinyScriptParser.ArgumentContext context)
        {
            if (context.expression() != null)
            {
                string expr = VisitExpression(context.expression());
                return $"({ expr })";
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

        public override string VisitValue([NotNull] TinyScriptParser.ValueContext context)
        {
            return context.GetText();
        }

        public override string VisitIfStatement([NotNull] TinyScriptParser.IfStatementContext context)
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

        public override string VisitElseIfStatement([NotNull] TinyScriptParser.ElseIfStatementContext context)
        {
            string result = $"else if ({ VisitExpression(context.expression()) }) ";
            result += VisitBlock(context.block());
            return result;
        }

        public override string VisitElseStatement([NotNull] TinyScriptParser.ElseStatementContext context)
        {
            string result = $"else ";
            result += VisitBlock(context.block());
            return result;
        }

        public override string VisitBlock(TinyScriptParser.BlockContext context)
        {
            string statements = VisitStatementList(context.statementList());
            Indent(ref statements);
            return $"{{\n{ statements }\n}}";
        }

        public override string VisitWhileStatement([NotNull] TinyScriptParser.WhileStatementContext context)
        {
            string result = $"while ({ VisitExpression(context.expression()) }) ";
            result += VisitBlock(context.block());
            return result;
        }

        public override string VisitDoWhileStatement([NotNull] TinyScriptParser.DoWhileStatementContext context)
        {
            string result = "do ";
            result += VisitBlock(context.block());
            result += $"\nwhile ({ VisitExpression(context.expression()) });";
            return result;
        }

        public override string VisitForStatement([NotNull] TinyScriptParser.ForStatementContext context)
        {
            string varName = context.varName().GetText();
            string assignExpr = VisitExpression(context.expression()[0]);
            string cond = VisitExpression(context.expression()[1]);
            string incr = VisitIncrementation(context.incrementation());
            string result = $"for ({ varName } = { assignExpr }; { cond }; { incr }) ";
            result += VisitBlock(context.block());
            return result;
        }

        public override string VisitIncrementation([NotNull] TinyScriptParser.IncrementationContext context)
        {
            string varName = context.varName().GetText();
            string op;
            if (context.expression() == null)
            {
                op = context.INCDEC1().GetText();
                return $"{ varName }{ op }";
            }
            op = context.INCDEC2().GetText();
            string expr = VisitExpression(context.expression());
            return $"{ varName } { op } { expr }";
        }

        public override string VisitIncrementStatement([NotNull] TinyScriptParser.IncrementStatementContext context)
        {
            return VisitIncrementation(context.incrementation()) + ";";
        }

        public override string VisitAssignmentStatement([NotNull] TinyScriptParser.AssignmentStatementContext context)
        {
            string varName = context.varName().GetText();
            string expr = VisitExpression(context.expression());
            return $"{ varName } = { expr };";
        }

        public override string VisitFunctionCallStatement([NotNull] TinyScriptParser.FunctionCallStatementContext context)
        {
            return VisitFunctionCall(context.functionCall()) + ";";
        }

        public override string VisitFunctionCall([NotNull] TinyScriptParser.FunctionCallContext context)
        {
            string name = context.functionName().GetText();
            var expression = context.expression();
            switch (name)
            {
                case "print": return PrintFunction(expression[0]);
                case "min": return MinMaxFunction(expression, false);
                case "max": return MinMaxFunction(expression, true);
            }
            string result = $"{ name }(";
            for (int i = 0; i < expression.Length; i++)
            {
                if (i > 0)
                {
                    result += ", ";
                }
                result += VisitExpression(expression[i]);
            }
            result += ")";
            return result;
        }

        private string PrintFunction(TinyScriptParser.ExpressionContext exprContext)
        {
            string expression = VisitExpression(exprContext);
            return $"std::cout << ({ expression }) << std::endl";
        }

        private string MinMaxFunction(TinyScriptParser.ExpressionContext[] exprContext, bool max)
        {
            string result = max ? "Max" : "Min";
            result += $"({ exprContext.Length }";
            foreach (var expr in exprContext)
            {
                result += $", { VisitExpression(expr) }";
            }
            result += ")";
            return result;
        }

        public override string VisitArrayDeclaration([NotNull] TinyScriptParser.ArrayDeclarationContext context)
        {
            string varName = context.varName().GetText();
            VariableType type = typeData.GetVariableType(varName);
            return $"{ type.ToCPPTypeName() } { varName }({ type.Size });";
        }

        public override string VisitArrayAssignmentStatement([NotNull] TinyScriptParser.ArrayAssignmentStatementContext context)
        {
            string varName = context.varName().GetText();
            string expr1 = VisitExpression(context.expression()[0]);
            string expr2 = VisitExpression(context.expression()[1]);
            return $"{ varName }[{ expr1 }] = { expr2 };";
        }

        public override string VisitIndexedArray([NotNull] TinyScriptParser.IndexedArrayContext context)
        {
            string varName = context.varName().GetText();
            string expr = VisitExpression(context.expression());
            return $"{ varName }[{ expr }]";
        }

        public override string VisitArrayInitialization([NotNull] TinyScriptParser.ArrayInitializationContext context)
        {
            string varName = context.varName().GetText();
            VariableType type = typeData.GetVariableType(varName);
            string result = $"{ type.ElementType.ToCPPTypeName() } _{ varName }_helper[] = {{ ";
            for (int i = 0; i < context.expression().Length; i++)
            {
                if (i > 0)
                {
                    result += ", ";
                }
                result += VisitExpression(context.expression()[i]);
            }
            result += $" }};\n{ type.ToCPPTypeName() } { varName }(_{ varName }_helper, _{ varName }_helper + { type.Size });";
            return result;
        }

        public override string VisitReadStatement([NotNull] TinyScriptParser.ReadStatementContext context)
        {
            string varName = context.varName().GetText();
            return $"std::cin >> { varName };";
        }

        public override string VisitCountStatement([NotNull] TinyScriptParser.CountStatementContext context)
        {
            string varName = context.varName()[0].GetText();
            string from = context.@int()[0].GetText(); ;
            string to = context.@int()[0].GetText(); ;
            string incr = VisitIncrementation(context.incrementation());
            string result = $"for ({ varName } = { from }; { varName } <= { to }; { incr }) ";
            result += VisitBlock(context.block());
            return result;
        }

        public override string VisitFunctionDefinition([NotNull] TinyScriptParser.FunctionDefinitionContext context)
        {
            string functionName = context.functionName().GetText();
            typeData.EnterScope(functionName);
            VariableType retType = typeData.GetVariableType("_return");
            string result = $"{ retType.ToCPPTypeName() } { functionName }(";
            var parameters = context.parameter();
            for (int i = 0; i < parameters.Length; i++)
            {
                if (i > 0)
                {
                    result += ", ";
                }
                string name = parameters[i].varName().GetText();
                VariableType type = typeData.GetVariableType(name);
                if (type.IsArray)
                {
                    name = "&" + name;
                }
                result += $"{ type.ToCPPTypeName() } { name }";
            }
            result += ")\n";
            result += VisitFunctionBody(context.functionBody());
            typeData.ExitScope();
            return result;
        }

        public override string VisitFunctionBody([NotNull] TinyScriptParser.FunctionBodyContext context)
        {
            string declaration = VisitVariableDeclarationList(context.variableDeclarationList());
            Indent(ref declaration);
            string statements = VisitFunctionStatementList(context.functionStatementList());
            Indent(ref statements);
            return $"{{\n{ declaration }\n\n{ statements }\n}}";
        }

        private static void Indent(ref string str)
        {
            str = "\t" + str.Replace("\n", "\n\t");
        }
    }
}
