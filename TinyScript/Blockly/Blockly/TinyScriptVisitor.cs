using Antlr4.Runtime;
using Antlr4.Runtime.Misc;
using Antlr4.Runtime.Tree;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blockly
{
    public class TinyScriptVisitor : TinyScriptBaseVisitor<VariableType>
    {
        public class TypeData
        {
            private Dictionary<string, VariableType> variables;

            public TypeData(Dictionary<string, VariableType> variables)
            {
                this.variables = new Dictionary<string, VariableType>(variables);
            }

            public VariableType GetVariableType(string name)
            {
                return variables[name];
            }
        }

        private Dictionary<string, VariableType> variables = new Dictionary<string, VariableType>();

        private void ThrowSyntaxError(IToken token, string message)
        {
            throw new SyntaxErrorException(token.Line, token.Column, message);
        }

        public TypeData Analyze(TinyScriptParser.ProgramContext context)
        {
            VisitProgram(context);
            return new TypeData(variables);
        }

        public override VariableType VisitProgram([NotNull] TinyScriptParser.ProgramContext context)
        {
            VisitVariableDeclarationList(context.variableDeclarationList());
            VisitStatementList(context.statementList());
            return VariableType.VOID;
        }

        public override VariableType VisitVariableDeclarationList([NotNull] TinyScriptParser.VariableDeclarationListContext context)
        {
            foreach (var declaration in context.variableDeclaration())
            {
                VisitVariableDeclaration(declaration);
            }
            return VariableType.VOID;
        }

        public override VariableType VisitStatementList([NotNull] TinyScriptParser.StatementListContext context)
        {
            foreach (var statement in context.statement())
            {
                VisitStatement(statement);
            }
            return VariableType.VOID;
        }

        public override VariableType VisitStatement([NotNull] TinyScriptParser.StatementContext context)
        {
            return Visit(context.GetChild(0));
        }

        public override VariableType VisitVariableDeclaration([NotNull] TinyScriptParser.VariableDeclarationContext context)
        {
            return Visit(context.GetChild(0));
        }

        public override VariableType VisitVariableDeclaration1([NotNull] TinyScriptParser.VariableDeclaration1Context context)
        {
            string typeName = context.typeName().GetText();
            VariableType type = VariableType.FromString(typeName);
            VariableType expression = null;
            if (context.expression() != null)
            {
                expression = VisitExpression(context.expression());
                if (!CheckType(type, expression))
                {
                    ThrowSyntaxError(context.expression().Start, "Type mismatch");
                }
            }
            MakeVariable(type, context.varName().Start, false);
            return VariableType.VOID;
        }

        public override VariableType VisitVariableDeclaration2([NotNull] TinyScriptParser.VariableDeclaration2Context context)
        {
            VariableType expression = VisitExpression(context.expression());
            if (expression == VariableType.NULL)
            {
                ThrowSyntaxError(context.expression().Start, "Type mismatch");
            }
            MakeVariable(expression, context.varName().Start, true);
            return VariableType.VOID;
        }

        private void MakeVariable(VariableType type, IToken name, bool var)
        {
            string varName = name.Text;
            if (variables.ContainsKey(varName))
            {
                ThrowSyntaxError(name, "Variable already exists");
            }
            variables.Add(varName, type);
        }

        public override VariableType VisitExpression([NotNull] TinyScriptParser.ExpressionContext context)
        {
            if (context.compareOp() == null)
            {
                return VisitSum(context.sum()[0]);
            }
            VariableType leftExpr = VisitSum(context.sum()[0]);
            VariableType rightExpr = VisitSum(context.sum()[1]);
            if (leftExpr != VariableType.INT || rightExpr != VariableType.INT)
            {
                ThrowSyntaxError(context.compareOp().Start, "Type mismatch");
            }
            return VariableType.BOOLEAN;
        }

        public override VariableType VisitSum([NotNull] TinyScriptParser.SumContext context)
        {
            return Expression(context.PLUSMINUS(), context.product());
        }

        private VariableType Expression(IEnumerable<ITerminalNode> ops, IEnumerable<TinyScriptParser.ProductContext> products)
        {
            if (ops.Count() == 0)
            {
                return VisitProduct(products.ElementAt(0));
            }
            VariableType leftExpr = VisitProduct(products.ElementAt(0));
            VariableType rightExpr = Expression(ops.Skip(1), products.Skip(1));
            if (leftExpr != VariableType.INT || rightExpr != VariableType.INT)
            {
                ThrowSyntaxError(ops.ElementAt(0).Symbol, "Type mismatch");
            }
            return VariableType.INT;
        }

        public override VariableType VisitProduct([NotNull] TinyScriptParser.ProductContext context)
        {
            return Product(context.MULDIV(), context.signedArgument());
        }

        private VariableType Product(IEnumerable<ITerminalNode> ops, IEnumerable<TinyScriptParser.SignedArgumentContext> args)
        {
            if (ops.Count() == 0)
            {
                return VisitSignedArgument(args.ElementAt(0));
            }
            VariableType leftExpr = VisitSignedArgument(args.ElementAt(0));
            VariableType rightExpr = Product(ops.Skip(1), args.Skip(1));
            if (leftExpr != VariableType.INT || rightExpr != VariableType.INT)
            {
                ThrowSyntaxError(ops.ElementAt(0).Symbol, "Type mismatch");
            }
            return VariableType.INT;
        }

        public override VariableType VisitSignedArgument([NotNull] TinyScriptParser.SignedArgumentContext context)
        {
            ITerminalNode op = context.PLUSMINUS();
            if (op == null || op.GetText() == "+")
            {
                return VisitArgument(context.argument());
            }
            VariableType type = VisitArgument(context.argument());
            if (type != VariableType.INT)
            {
                ThrowSyntaxError(context.argument().Start, "Type mismatch");
            }
            return type;
        }

        public override VariableType VisitArgument([NotNull] TinyScriptParser.ArgumentContext context)
        {
            if (context.expression() != null)
            {
                return VisitExpression(context.expression());
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
                return VisitVarName(context.varName());
            }
            return VisitValue(context.value());
        }

        public override VariableType VisitVarName([NotNull] TinyScriptParser.VarNameContext context)
        {
            string varName = context.GetText();
            if (!variables.ContainsKey(varName))
            {
                ThrowSyntaxError(context.Start, "Variable does not exist");
            }
            return variables[varName];
        }

        public override VariableType VisitValue([NotNull] TinyScriptParser.ValueContext context)
        {
            if (context.INT() != null)
            {
                return VariableType.INT;
            }
            if (context.BOOLEAN() != null)
            {
                return VariableType.BOOLEAN;
            }
            if (context.STRING() != null)
            {
                return VariableType.STRING;
            }
            return VariableType.NULL;
        }

        private void Conditional(TinyScriptParser.ExpressionContext expressionContext)
        {
            VariableType expression = VisitExpression(expressionContext);
            if (expression != VariableType.BOOLEAN)
            {
                ThrowSyntaxError(expressionContext.Start, "Type mismatch");
            }
        }

        public override VariableType VisitIfStatement([NotNull] TinyScriptParser.IfStatementContext context)
        {
            Conditional(context.expression());
            var elseIfs = context.elseIfStatement();
            for (int i = 0; i < elseIfs.Length; i++)
            {
                Conditional(elseIfs[i].expression());
            }
            return VariableType.VOID;
        }

        public override VariableType VisitWhileStatement([NotNull] TinyScriptParser.WhileStatementContext context)
        {
            Conditional(context.expression());
            return VariableType.VOID;
        }

        public override VariableType VisitDoWhileStatement([NotNull] TinyScriptParser.DoWhileStatementContext context)
        {
            Conditional(context.expression());
            return VariableType.VOID;
        }

        public override VariableType VisitBlock([NotNull] TinyScriptParser.BlockContext context)
        {
            return VisitStatementList(context.statementList());
        }

        public override VariableType VisitForStatement([NotNull] TinyScriptParser.ForStatementContext context)
        {
            VariableType type = VisitVarName(context.varName());
            if (type != VariableType.INT)
            {
                ThrowSyntaxError(context.varName().Start, "Type mismatch");
            }
            VariableType assignExpr = VisitExpression(context.expression()[0]);
            VariableType compareExpr = VisitExpression(context.expression()[1]);
            if (assignExpr != VariableType.INT || compareExpr != VariableType.BOOLEAN)
            {
                ThrowSyntaxError(context.expression()[0].Start, "Type mismatch");
            }
            VisitIncrementation(context.incrementation());
            return VariableType.VOID;
        }

        public override VariableType VisitIncrementation([NotNull] TinyScriptParser.IncrementationContext context)
        {
            VariableType type = VisitVarName(context.varName());
            if (type != VariableType.INT)
            {
                ThrowSyntaxError(context.varName().Start, "Type mismatch");
            }
            if (context.expression() != null)
            {
                VariableType expression = VisitExpression(context.expression());
                if (expression != VariableType.INT)
                {
                    ThrowSyntaxError(context.expression().Start, "Type mismatch");
                }
            }
            return VariableType.VOID;
        }

        public override VariableType VisitIncrementStatement([NotNull] TinyScriptParser.IncrementStatementContext context)
        {
            return VisitIncrementation(context.incrementation());
        }

        public override VariableType VisitAssignmentStatement([NotNull] TinyScriptParser.AssignmentStatementContext context)
        {
            VariableType type = VisitVarName(context.varName());
            VariableType expr = VisitExpression(context.expression());
            if (!CheckType(expr, type))
            {
                ThrowSyntaxError(context.expression().Start, "Type mismatch");
            }
            return VariableType.VOID;
        }

        public override VariableType VisitFunctionCallStatement([NotNull] TinyScriptParser.FunctionCallStatementContext context)
        {
            return VisitFunctionCall(context.functionCall());
        }

        public override VariableType VisitFunctionCall([NotNull] TinyScriptParser.FunctionCallContext context)
        {
            string functionName = context.functionName().GetText();
            IToken nameToken = context.functionName().Start;
            switch (functionName)
            {
                case "print": return PrintFunction(nameToken, context.expression());
                case "abs": return AbsFunction(nameToken, context.expression());
                case "min": return MinMaxFunction(nameToken, context.expression(), false);
                case "max": return MinMaxFunction(nameToken, context.expression(), true);
                default: ThrowSyntaxError(nameToken, "Function does not exist"); return null;
            }
        }

        private VariableType PrintFunction(IToken nameToken, TinyScriptParser.ExpressionContext[] args)
        {
            if (args.Length != 1)
            {
                ThrowSyntaxError(nameToken, "Invalid argument list");
            }
            VariableType expression = VisitExpression(args[0]);
            if (expression.IsArray)
            {
                ThrowSyntaxError(args[0].Start, "Type mismatch");
            }
            return VariableType.VOID;
        }

        private VariableType AbsFunction(IToken nameToken, TinyScriptParser.ExpressionContext[] args)
        {
            if (args.Length != 1)
            {
                ThrowSyntaxError(nameToken, "Invalid argument list");
            }
            VariableType expression = VisitExpression(args[0]);
            if (expression != VariableType.INT)
            {
                ThrowSyntaxError(args[0].Start, "Type mismatch");
            }
            return VariableType.INT;
        }

        private VariableType MinMaxFunction(IToken nameToken, TinyScriptParser.ExpressionContext[] args, bool max)
        {
            if (args.Length == 0)
            {
                ThrowSyntaxError(nameToken, "Invalid argument list");
            }
            for (int i = 0; i < args.Length; i++)
            {
                VariableType expr = VisitExpression(args[i]);
                if (expr != VariableType.INT)
                {
                    ThrowSyntaxError(args[i].Start, "Type mismatch");
                }
            }
            return VariableType.INT;
        }

        private static bool CheckType(VariableType type1, VariableType type2)
        {
            if (type1 == VariableType.NULL || type2 == VariableType.NULL)
            {
                return true;
            }
            return type1 == type2;
        }

        public override VariableType VisitArrayDeclaration([NotNull] TinyScriptParser.ArrayDeclarationContext context)
        {
            VariableType type = VariableType.ArrayFromString(context.typeName().GetText(), 0);
            VariableType expr = VisitExpression(context.expression());
            if (expr != VariableType.INT)
            {
                ThrowSyntaxError(context.expression().Start, "Type mismatch");
            }
            MakeVariable(type, context.varName().Start, false);
            return VariableType.VOID;
        }

        public override VariableType VisitArrayAssignmentStatement([NotNull] TinyScriptParser.ArrayAssignmentStatementContext context)
        {
            VariableType type = VisitVarName(context.varName());
            VariableType idxExpression = VisitExpression(context.expression()[0]);
            if (idxExpression != VariableType.INT)
            {
                ThrowSyntaxError(context.expression()[0].Start, "Type mismatch");
            }
            VariableType valueExpression = VisitExpression(context.expression()[1]);
            if (!type.IsArray || valueExpression != type.ElementType)
            {
                ThrowSyntaxError(context.expression()[1].Start, "Type mismatch");
            }
            return VariableType.VOID;
        }

        public override VariableType VisitIndexedArray([NotNull] TinyScriptParser.IndexedArrayContext context)
        {
            VariableType type = VisitVarName(context.varName());
            VariableType idxExpression = VisitExpression(context.expression());
            if (!type.IsArray || idxExpression != VariableType.INT)
            {
                ThrowSyntaxError(context.expression().Start, "Type mismatch");
            }
            return type.ElementType;
        }

        public override VariableType VisitArrayInitialization([NotNull] TinyScriptParser.ArrayInitializationContext context)
        {
            var expressions = context.expression();
            VariableType type = VariableType.ArrayFromString(context.typeName().GetText(), expressions.Length);
            MakeVariable(type, context.varName().Start, false);
            for (int i = 0; i < expressions.Length; i++)
            {
                VariableType expression = VisitExpression(expressions[i]);
                if (!CheckType(expression, type.ElementType))
                {
                    ThrowSyntaxError(expressions[i].Start, "Type mismatch");
                }
            }
            return VariableType.VOID;
        }

        public override VariableType VisitReadStatement([NotNull] TinyScriptParser.ReadStatementContext context)
        {
            VariableType type = VisitVarName(context.varName());
            if (type.IsArray)
            {
                ThrowSyntaxError(context.varName().Start, "Type mismatch");
            }
            return VariableType.VOID;
        }
    }
}
