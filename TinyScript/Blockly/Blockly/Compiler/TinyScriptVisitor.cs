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
        public interface ITypeData
        {
            VariableType GetVariableType(string name);

            int GetParameterCount();

            string GetParameterName(int i);

            void EnterScope(string scope);

            void ExitScope();
        }

        private class TypeData : ITypeData
        {
            private Dictionary<string, Dictionary<string, VariableType>> variables = new Dictionary<string, Dictionary<string, VariableType>>();
            private Dictionary<string, List<string>> parameters = new Dictionary<string, List<string>>();
            private string scope;

            public TypeData()
            {
                TryAddScope("_global");
                EnterScope("_global");
                TryAddVariable("_return", VariableType.VOID);
            }

            public VariableType GetVariableType(string name)
            {
                return variables[scope][name];
            }

            public bool TryGetVariableType(string name, out VariableType type)
            {
                return variables[scope].TryGetValue(name, out type);
            }

            public bool TryAddVariable(string name, VariableType type)
            {
                if (variables[scope].ContainsKey(name))
                {
                    return false;
                }
                variables[scope].Add(name, type);
                return true;
            }

            public bool TryAddScope(string scope)
            {
                if (variables.ContainsKey(scope))
                {
                    return false;
                }
                variables.Add(scope, new Dictionary<string, VariableType>());
                parameters.Add(scope, new List<string>());
                return true;
            }

            public void EnterScope(string scope)
            {
                this.scope = scope;
            }

            public bool TryEnterScope(string scope)
            {
                if (!variables.ContainsKey(scope))
                {
                    return false;
                }
                EnterScope(scope);
                return true;
            }

            public void ExitScope()
            {
                EnterScope("_global");
            }

            public int GetParameterCount()
            {
                return parameters[scope].Count;
            }

            public void AddParameter(string name)
            {
                parameters[scope].Add(name);
            }

            public string GetParameterName(int i)
            {
                return parameters[scope][i];
            }
        }

        private TypeData typeData = new TypeData();

        private void ThrowSyntaxError(IToken token, string message)
        {
            throw new SyntaxErrorException(token.Line, token.Column, message);
        }

        private void ThrowSyntaxErrorCannotConvert(IToken token, VariableType typeConvertFrom, VariableType typeConvertTo)
        {
            ThrowSyntaxError(token, $"Cannot convert '{typeConvertFrom}' to '{typeConvertTo}'");
        }

        private void ThrowSyntaxErrorOperator(IToken token, string op, VariableType leftExpr, VariableType rightExpr)
        {
            if (leftExpr == VariableType.VOID)
            {
                ThrowSyntaxError(token, $"Operator '{op}' cannot be applied to operands of type '{rightExpr}' ");
            }
            ThrowSyntaxError(token, $"Operator '{op}' cannot be applied to operands of type '{leftExpr}' and '{rightExpr}' ");
        }

        public ITypeData Analyze(TinyScriptParser.ProgramContext context)
        {
            VisitProgram(context);
            return typeData;
        }

        public override VariableType VisitProgram([NotNull] TinyScriptParser.ProgramContext context)
        {
            VisitFunctionDefinitionList(context.functionDefinitionList());
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

        public override VariableType VisitFunctionDefinitionList([NotNull] TinyScriptParser.FunctionDefinitionListContext context)
        {
            foreach (var def in context.functionDefinition())
            {
                VisitFunctionDefinition(def);
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
                if (type != expression)
                {
                    ThrowSyntaxErrorCannotConvert(context.expression().Start, expression, type);
                }
            }
            MakeVariable(type, context.varName().Start);
            return VariableType.VOID;
        }

        public override VariableType VisitVariableDeclaration2([NotNull] TinyScriptParser.VariableDeclaration2Context context)
        {
            VariableType expression = VisitExpression(context.expression());
            MakeVariable(new PrimitiveType(expression.Name), context.varName().Start);
            return VariableType.VOID;
        }

        private void MakeVariable(VariableType type, IToken name)
        {
            string varName = name.Text;
            if (!typeData.TryAddVariable(varName, type))
            {
                ThrowSyntaxError(name, "Variable already exists");
            }
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
                ThrowSyntaxErrorOperator(context.compareOp().Start, context.compareOp().GetText(), leftExpr, rightExpr);
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
                ThrowSyntaxErrorOperator(ops.ElementAt(0).Symbol, ops.ElementAt(0).GetText(), leftExpr, rightExpr);
            }
            int value1, value2;
            if (leftExpr.TryGetValue(out value1) && rightExpr.TryGetValue(out value2))
            {
                bool add = ops.ElementAt(0).GetText() == "+";
                return new Constant(add ? value1 + value2 : value1 - value2);
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
                ThrowSyntaxErrorOperator(ops.ElementAt(0).Symbol, ops.ElementAt(0).GetText(), leftExpr, rightExpr);
            }
            int value1, value2;
            if (leftExpr.TryGetValue(out value1) && rightExpr.TryGetValue(out value2))
            {
                bool mul = ops.ElementAt(0).GetText() == "*";
                return new Constant(mul ? value1 * value2 : value1 / value2);
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
                ThrowSyntaxErrorOperator(context.argument().Start, op.GetText(), VariableType.VOID, type);
            }
            int value;
            if (type.TryGetValue(out value))
            {
                return new Constant(-value);
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
            VariableType type;
            if (!typeData.TryGetVariableType(varName, out type))
            {
                ThrowSyntaxError(context.Start, "Variable does not exist");
            }
            return type;
        }

        public override VariableType VisitValue([NotNull] TinyScriptParser.ValueContext context)
        {
            if (context.INT() != null)
            {
                return new Constant(int.Parse(context.GetText()));
            }
            if (context.BOOLEAN() != null)
            {
                return VariableType.BOOLEAN;
            }
            return VariableType.STRING;
        }

        private void Conditional(TinyScriptParser.ExpressionContext expressionContext)
        {
            VariableType expression = VisitExpression(expressionContext);
            if (expression != VariableType.BOOLEAN)
            {
                ThrowSyntaxErrorCannotConvert(expressionContext.Start, expression, VariableType.BOOLEAN);
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
                ThrowSyntaxErrorCannotConvert(context.varName().Start, type, VariableType.INT);
            }
            VariableType assignExpr = VisitExpression(context.expression()[0]);
            VariableType compareExpr = VisitExpression(context.expression()[1]);
            if (assignExpr != VariableType.INT)
            {
                ThrowSyntaxErrorCannotConvert(context.expression()[0].Start, assignExpr, VariableType.INT);
            }
            if (compareExpr != VariableType.BOOLEAN)
            {
                ThrowSyntaxErrorCannotConvert(context.expression()[1].Start, compareExpr, VariableType.BOOLEAN);
            }
            VisitIncrementation(context.incrementation());
            return VariableType.VOID;
        }

        public override VariableType VisitIncrementation([NotNull] TinyScriptParser.IncrementationContext context)
        {
            VariableType type = VisitVarName(context.varName());
            if (type != VariableType.INT)
            {
                if (context.INCDEC1() != null)
                {
                    ThrowSyntaxErrorOperator(context.varName().Start, context.INCDEC1().GetText(), VariableType.VOID, type);
                }
                else
                {
                    ThrowSyntaxErrorOperator(context.varName().Start, context.INCDEC2().GetText(), VariableType.VOID, type);
                }
            }
            if (context.expression() != null)
            {
                VariableType expression = VisitExpression(context.expression());
                if (expression != VariableType.INT)
                {
                    ThrowSyntaxErrorCannotConvert(context.expression().Start, expression, VariableType.INT);
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
            if (expr != type)
            {
                ThrowSyntaxErrorCannotConvert(context.expression().Start, expr, type);
            }
            return VariableType.VOID;
        }

        public override VariableType VisitFunctionCallStatement([NotNull] TinyScriptParser.FunctionCallStatementContext context)
        {
            return VisitFunctionCall(context.functionCall());
        }

        public override VariableType VisitFunctionCall([NotNull] TinyScriptParser.FunctionCallContext context)
        {
            IToken nameToken = context.functionName().Start;
            switch (nameToken.Text)
            {
                case "print": return PrintFunction(nameToken, context.expression());
                case "abs": return AbsFunction(nameToken, context.expression());
                case "min": return MinMaxFunction(nameToken, context.expression(), false);
                case "max": return MinMaxFunction(nameToken, context.expression(), true);
                default: return CustomFunction(nameToken, context.expression());
            }
        }

        private VariableType CustomFunction(IToken nameToken, TinyScriptParser.ExpressionContext[] args)
        {
            if (!typeData.TryEnterScope(nameToken.Text))
            {
                ThrowSyntaxError(nameToken, "Function does not exist");
            }
            if (args.Length != typeData.GetParameterCount())
            {
                ThrowSyntaxError(nameToken, "Invalid argument list");
            }
            for (int i = 0; i < args.Length; i++)
            {
                VariableType type = typeData.GetVariableType(typeData.GetParameterName(i));
                VariableType expr = VisitExpression(args[i]);
                if (type != expr)
                {
                    ThrowSyntaxErrorCannotConvert(args[i].Start, type, expr);
                }
            }
            VariableType result = typeData.GetVariableType("_return");
            typeData.ExitScope();
            return result;
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
                ThrowSyntaxError(args[0].Start, "Array cannot be printed");
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
                ThrowSyntaxErrorCannotConvert(args[0].Start, expression, VariableType.INT);
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
                    ThrowSyntaxErrorCannotConvert(args[i].Start, expr, VariableType.INT);
                }
            }
            return VariableType.INT;
        }

        public override VariableType VisitArrayDeclaration([NotNull] TinyScriptParser.ArrayDeclarationContext context)
        {
            VariableType expr = VisitExpression(context.expression());
            int size;
            if (!expr.TryGetValue(out size))
            {
                ThrowSyntaxErrorCannotConvert(context.expression().Start, expr, VariableType.INT);
            }
            if (size < 0)
            {
                ThrowSyntaxError(context.expression().Start, "Array size must be positive or zero");
            }
            VariableType type = VariableType.ArrayFromString(context.typeName().GetText(), size);
            MakeVariable(type, context.varName().Start);
            return VariableType.VOID;
        }

        public override VariableType VisitArrayAssignmentStatement([NotNull] TinyScriptParser.ArrayAssignmentStatementContext context)
        {
            VariableType type = VisitVarName(context.varName());
            VariableType idxExpression = VisitExpression(context.expression()[0]);
            if (idxExpression != VariableType.INT)
            {
                ThrowSyntaxErrorCannotConvert(context.expression()[0].Start, idxExpression, type);
            }
            VariableType valueExpression = VisitExpression(context.expression()[1]);
            if (!type.IsArray || valueExpression != type.ElementType)
            {
                ThrowSyntaxErrorCannotConvert(context.expression()[1].Start, valueExpression, type.ElementType);
            }
            return VariableType.VOID;
        }

        public override VariableType VisitIndexedArray([NotNull] TinyScriptParser.IndexedArrayContext context)
        {
            VariableType type = VisitVarName(context.varName());
            VariableType idxExpression = VisitExpression(context.expression());
            if (!type.IsArray || idxExpression != VariableType.INT)
            {
                ThrowSyntaxErrorCannotConvert(context.expression().Start, idxExpression, VariableType.INT);
            }
            return type.ElementType;
        }

        public override VariableType VisitArrayInitialization([NotNull] TinyScriptParser.ArrayInitializationContext context)
        {
            var expressions = context.expression();
            VariableType type = VariableType.ArrayFromString(context.typeName().GetText(), expressions.Length);
            MakeVariable(type, context.varName().Start);
            for (int i = 0; i < expressions.Length; i++)
            {
                VariableType expression = VisitExpression(expressions[i]);
                if (expression != type.ElementType)
                {
                    ThrowSyntaxErrorCannotConvert(expressions[i].Start, expression, type.ElementType);
                }
            }
            return VariableType.VOID;
        }

        public override VariableType VisitReadStatement([NotNull] TinyScriptParser.ReadStatementContext context)
        {
            VariableType type = VisitVarName(context.varName());
            if (type.IsArray)
            {
                ThrowSyntaxError(context.varName().Start, "Input cannot be read into array");
            }
            return VariableType.VOID;
        }

        public override VariableType VisitFunctionDefinition([NotNull] TinyScriptParser.FunctionDefinitionContext context)
        {
            string functionName = context.functionName().GetText();
            if (!typeData.TryAddScope(functionName))
            {
                ThrowSyntaxError(context.functionName().Start, "Function already exists");
            }
            typeData.EnterScope(functionName);
            VariableType type;
            foreach (var param in context.parameter())
            {
                if (param.BRACKET3() != null)
                {
                    type = VariableType.ArrayFromString(param.typeName().GetText(), -1);
                }
                else
                {
                    type = VariableType.FromString(param.typeName().GetText());
                }
                if (!typeData.TryAddVariable(param.varName().GetText(), type))
                {
                    ThrowSyntaxError(param.Start, "Parameter already exists");
                }
                typeData.AddParameter(param.varName().GetText());
            }
            if (context.typeName() != null)
            {
                if (context.BRACKET3() != null)
                {
                    type = VariableType.ArrayFromString(context.typeName().GetText(), -1);
                }
                else
                {
                    type = VariableType.FromString(context.typeName().GetText());
                }
            }
            else
            {
                type = VariableType.VOID;
            }
            typeData.TryAddVariable("_return", type);
            VisitFunctionBody(context.functionBody());
            typeData.ExitScope();
            return VariableType.VOID;
        }

        public override VariableType VisitFunctionBody([NotNull] TinyScriptParser.FunctionBodyContext context)
        {
            VisitVariableDeclarationList(context.variableDeclarationList());
            VisitStatementList(context.statementList());
            return VariableType.VOID;
        }

        public override VariableType VisitReturnStatement([NotNull] TinyScriptParser.ReturnStatementContext context)
        {
            VariableType expression = VisitExpression(context.expression());
            VariableType type = typeData.GetVariableType("_return");
            if (expression != type)
            {
                ThrowSyntaxErrorCannotConvert(context.expression().Start, expression, type);
            }
            return VariableType.VOID;
        }

        public override VariableType VisitCountStatement([NotNull] TinyScriptParser.CountStatementContext context)
        {
            string varName = context.varName()[0].GetText();
            if (varName != context.varName()[1].GetText() || varName != context.countIncrementation().varName().GetText())
            {
                ThrowSyntaxError(context.varName()[0].Start, "Variables must be the same");
            }
            VariableType type = VisitVarName(context.varName()[0]);
            if (type != VariableType.INT)
            {
                ThrowSyntaxErrorCannotConvert(context.varName()[0].Start, type, VariableType.INT);
            }
            return VariableType.VOID;
        }
    }
}
