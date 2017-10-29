using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using Antlr4.Runtime.Misc;
using Antlr4.Runtime.Tree;
using Antlr4.Runtime;

namespace Blockly
{
    public enum VariableType
    {
        INT, BOOLEAN, STRING, NULL
    }

    public class ExpressionElement : XElement
    {
        public VariableType Type { get; private set; }

        public ExpressionElement(XElement elem, VariableType type) : base(elem)
        {
            Type = type;
        }
    }

    public class TinyScriptVisitor : TinyScriptBaseVisitor<XElement>
    {
        private Dictionary<string, VariableType> variables = new Dictionary<string, VariableType>(); 

        private void ThrowSyntaxError(IToken token, string message)
        {
            throw new SyntaxErrorException(token.Line, token.Column, message);
        }

        public override XElement VisitProgram([NotNull] TinyScriptParser.ProgramContext context)
        {
            XElement xml = new XElement("xml");
            xml.Add(VisitVariableDeclarationList(context.variableDeclarationList()));
            xml.Add(VisitStatementList(context.statementList()));
            return xml;
        }

        public override XElement VisitVariableDeclarationList([NotNull] TinyScriptParser.VariableDeclarationListContext context)
        {
            return VisitList(context.variableDeclaration());
        }

        public override XElement VisitStatementList([NotNull] TinyScriptParser.StatementListContext context)
        {
            return VisitList(context.statement());
        }

        private XElement VisitList(IParseTree[] tree)
        {
            if (tree.Length == 0)
                return null;
            XElement root = Visit(tree[0]);
            XElement parent = root;
            for (int i = 1; i < tree.Length; i++)
            {
                XElement next = new XElement("next");
                parent.Add(next);
                XElement child = Visit(tree[i]);
                next.Add(child);
                parent = child;
            }
            return root;
        }

        public override XElement VisitStatement([NotNull] TinyScriptParser.StatementContext context)
        {
            return Visit(context.GetChild(0));
        }

        private VariableType GetTypeFromString(string str)
        {
            switch (str)
            {
                case "integer": return VariableType.INT;
                case "int": return VariableType.INT;
                case "string": return VariableType.STRING;
                case "boolean": return VariableType.BOOLEAN;
                case "bool": return VariableType.BOOLEAN;
                default: return VariableType.NULL;
            }
        }

        private string GetStringFromType(VariableType type)
        {
            switch (type)
            {
                case VariableType.INT: return "int";
                case VariableType.BOOLEAN: return "bool";
                case VariableType.STRING: return "string";
                default: return "";
            }
        }

        public override XElement VisitVariableDeclaration([NotNull] TinyScriptParser.VariableDeclarationContext context)
        {
            return Visit(context.GetChild(0));
        }

        public override XElement VisitVariableDeclaration1([NotNull] TinyScriptParser.VariableDeclaration1Context context)
        {
            string typeName = context.typeName().GetText();
            VariableType type = GetTypeFromString(typeName);
            ExpressionElement expression = null;
            if (context.expression() != null)
            {
                expression = (ExpressionElement)VisitExpression(context.expression());
                if (!CheckType(type, expression.Type))
                {
                    ThrowSyntaxError(context.expression().Start, "Type mismatch");
                }
            }
            XElement block = MakeVariable(type, context.varName().Start, false);
            if (expression != null)
            {
                XElement value = new XElement("value", new XAttribute("name", "NAME"));
                value.Add(expression);
                block.Add(value);
            }
            return block;
        }

        public override XElement VisitVariableDeclaration2([NotNull] TinyScriptParser.VariableDeclaration2Context context)
        {
            ExpressionElement expression = (ExpressionElement)VisitExpression(context.expression());
            if (expression.Type == VariableType.NULL)
            {
                ThrowSyntaxError(context.expression().Start, "Type mismatch");
            }
            XElement block = MakeVariable(expression.Type, context.varName().Start, true);
            XElement value = new XElement("value", new XAttribute("name", "NAME"));
            value.Add(expression);
            block.Add(value);
            return block;
        }

        private XElement MakeVariable(VariableType type, IToken name, bool var)
        {
            string varName = name.Text;
            if (variables.ContainsKey(varName))
            {
                ThrowSyntaxError(name, "Variable already exists");
            }
            variables.Add(varName, type);
            XElement block = new XElement("block", new XAttribute("type", var ? "create_var" : "create_variabletype"));
            if (!var)
            {
                XElement typeField = new XElement("field", new XAttribute("name", "type"));
                typeField.Add(GetStringFromType(type));
                block.Add(typeField);
            }
            XElement nameField = new XElement("field", new XAttribute("name", "var"));
            nameField.Add(varName);
            block.Add(nameField);
            return block;
        }

        public override XElement VisitExpression([NotNull] TinyScriptParser.ExpressionContext context)
        {
            if (context.compareOp() == null)
            {
                return VisitSum(context.sum()[0]);
            }
            ExpressionElement leftExpr = (ExpressionElement)VisitSum(context.sum()[0]);
            ExpressionElement rightExpr = (ExpressionElement)VisitSum(context.sum()[1]);
            if (leftExpr.Type != VariableType.INT || rightExpr.Type != VariableType.INT)
            {
                ThrowSyntaxError(context.compareOp().Start, "Type mismatch");
            }
            XElement block = new XElement("block", new XAttribute("type", "logic_compare"));
            XElement field = new XElement("field", new XAttribute("name", "OP"));
            field.Value = GetCmpOperatorName(context.compareOp().GetText());
            block.Add(field);
            XElement left = new XElement("value", new XAttribute("name", "A"));
            left.Add(leftExpr);
            XElement right = new XElement("value", new XAttribute("name", "B"));
            right.Add(rightExpr);
            block.Add(left);
            block.Add(right);
            return new ExpressionElement(block, VariableType.BOOLEAN);
        }

        public override XElement VisitSum([NotNull] TinyScriptParser.SumContext context)
        {
            return Expression(context.PLUSMINUS(), context.product());
        }

        private XElement Expression(IEnumerable<ITerminalNode> ops, IEnumerable<TinyScriptParser.ProductContext> products)
        {
            if (ops.Count() == 0)
            {
                return VisitProduct(products.ElementAt(0));
            }
            XElement block = new XElement("block", new XAttribute("type", "math_arithmetic"));
            XElement field = new XElement("field", new XAttribute("name", "OP"));
            field.Value = ops.ElementAt(0).GetText() == "+" ? "ADD" : "MINUS";
            block.Add(field);
            XElement left = new XElement("value", new XAttribute("name", "A"));
            ExpressionElement leftExpr = (ExpressionElement)VisitProduct(products.ElementAt(0));
            left.Add(leftExpr);
            block.Add(left);
            XElement right = new XElement("value", new XAttribute("name", "B"));
            ExpressionElement rightExpr = (ExpressionElement)Expression(ops.Skip(1), products.Skip(1));
            right.Add(rightExpr);
            block.Add(right);
            if (leftExpr.Type != VariableType.INT || rightExpr.Type != VariableType.INT)
            {
                ThrowSyntaxError(ops.ElementAt(0).Symbol, "Type mismatch");
            }
            return new ExpressionElement(block, VariableType.INT);
        }

        public override XElement VisitProduct([NotNull] TinyScriptParser.ProductContext context)
        {
            return Product(context.MULDIV(), context.signedArgument());
        }

        private XElement Product(IEnumerable<ITerminalNode> ops, IEnumerable<TinyScriptParser.SignedArgumentContext> args)
        {
            if (ops.Count() == 0)
            {
                return VisitSignedArgument(args.ElementAt(0));
            }
            XElement block = new XElement("block", new XAttribute("type", "math_arithmetic"));
            XElement field = new XElement("field", new XAttribute("name", "OP"));
            field.Value = ops.ElementAt(0).GetText() == "*" ? "MULTIPLY" : "DIVIDE";
            block.Add(field);
            XElement left = new XElement("value", new XAttribute("name", "A"));
            ExpressionElement leftExpr = (ExpressionElement)VisitSignedArgument(args.ElementAt(0));
            left.Add(leftExpr);
            block.Add(left);
            XElement right = new XElement("value", new XAttribute("name", "B"));
            ExpressionElement rightExpr = (ExpressionElement)Product(ops.Skip(1), args.Skip(1));
            right.Add(rightExpr);
            block.Add(right);
            if (leftExpr.Type != VariableType.INT || rightExpr.Type != VariableType.INT)
            {
                ThrowSyntaxError(ops.ElementAt(0).Symbol, "Type mismatch");
            }
            return new ExpressionElement(block, VariableType.INT);
        }

        public override XElement VisitSignedArgument([NotNull] TinyScriptParser.SignedArgumentContext context)
        {
            ITerminalNode op = context.PLUSMINUS();
            if (op == null || op.GetText() == "+")
            {
                return VisitArgument(context.argument());
            }
            XElement block = new XElement("block", new XAttribute("type", "math_arithmetic"));
            XElement field = new XElement("field", new XAttribute("name", "OP"));
            field.Value = "MINUS";
            block.Add(field);
            XElement left = new XElement("value", new XAttribute("name", "A"));
            XElement zero = new XElement("block", new XAttribute("type", "math_number"));
            XElement zeroField = new XElement("field", new XAttribute("name", "NUM"));
            zeroField.Add("0");
            zero.Add(zeroField);
            left.Add(zero);
            block.Add(left);
            XElement right = new XElement("value", new XAttribute("name", "B"));
            ExpressionElement rightExpr = (ExpressionElement)VisitArgument(context.argument());
            right.Add(rightExpr);
            block.Add(right);
            return new ExpressionElement(block, rightExpr.Type);
        }

        public override XElement VisitArgument([NotNull] TinyScriptParser.ArgumentContext context)
        {
            if (context.expression() != null)
            {
                return VisitExpression(context.expression());
            }
            XElement block;
            XElement field;
            if (context.varName() != null)
            {
                string varName = context.varName().GetText();
                VariableType type = variables[varName];
                block = new XElement("block", new XAttribute("type", "variables_get"));
                field = new XElement("field", new XAttribute("name", "VAR"));
                field.Add(varName);
                block.Add(field);
                return new ExpressionElement(block, type);
            }
            return VisitValue(context.value());
        }

        public override XElement VisitValue([NotNull] TinyScriptParser.ValueContext context)
        {
            if (context.INT() != null)
            {
                XElement block = new XElement("block", new XAttribute("type", "math_number"));
                XElement field = new XElement("field", new XAttribute("name", "NUM"));
                field.Add(context.GetText());
                block.Add(field);
                return new ExpressionElement(block, VariableType.INT);
            }
            if (context.BOOLEAN() != null)
            {
                XElement block = new XElement("block", new XAttribute("type", "logic_boolean"));
                XElement field = new XElement("field", new XAttribute("name", "BOOL"));
                field.Add(context.GetText());
                block.Add(field);
                return new ExpressionElement(block, VariableType.BOOLEAN);
            }
            if (context.STRING() != null)
            {
                XElement block = new XElement("block", new XAttribute("type", "text"));
                XElement field = new XElement("field", new XAttribute("name", "TEXT"));
                field.Add(context.GetText().Trim('"'));
                block.Add(field);
                return new ExpressionElement(block, VariableType.STRING);
            }
            {
                XElement block = new XElement("block", new XAttribute("type", "logic_null"));
                return new ExpressionElement(block, VariableType.NULL);
            }
        }

        private void Conditional(XElement block, string condName, string blockName, TinyScriptParser.ExpressionContext expressionContext, TinyScriptParser.BlockContext blockContext)
        {
            XElement value = new XElement("value", new XAttribute("name", condName));
            ExpressionElement expression = (ExpressionElement)VisitExpression(expressionContext);
            if (expression.Type != VariableType.BOOLEAN)
            {
                ThrowSyntaxError(expressionContext.Start, "Type mismatch");
            }
            value.Add(expression);
            XElement statement = new XElement("statement", new XAttribute("name", blockName));
            statement.Add(VisitBlock(blockContext));
            block.Add(value);
            block.Add(statement);
        }

        public override XElement VisitIfStatement([NotNull] TinyScriptParser.IfStatementContext context)
        {
            XElement block = new XElement("block", new XAttribute("type", "controls_if"));
            Conditional(block, "IF0", "DO0", context.expression(), context.block());
            var elseIfs = context.elseIfStatement();
            XElement mutation = new XElement("mutation");
            mutation.Add(new XAttribute("elseif", elseIfs.Length));
            mutation.Add(new XAttribute("else", context.elseStatement() == null ? "0" : "1"));
            block.Add(mutation);
            for (int i = 0; i < elseIfs.Length; i++)
            {
                VisitElseIfStatement(elseIfs[i], block, i + 1);
            }
            if (context.elseStatement() != null)
            {
                block.Add(VisitElseStatement(context.elseStatement()));
            }
            return block;
        }

        private void VisitElseIfStatement(TinyScriptParser.ElseIfStatementContext context, XElement block, int num)
        {
            Conditional(block, $"IF{ num }", $"DO{ num }", context.expression(), context.block());
        }

        public override XElement VisitElseStatement([NotNull] TinyScriptParser.ElseStatementContext context)
        {
            XElement statement = new XElement("statement", new XAttribute("name", "ELSE"));
            statement.Add(VisitBlock(context.block()));
            return statement;
        }

        public override XElement VisitWhileStatement([NotNull] TinyScriptParser.WhileStatementContext context)
        {
            XElement block = new XElement("block", new XAttribute("type", "while"));
            Conditional(block, "condition", "statements", context.expression(), context.block());
            return block;
        }

        public override XElement VisitDoWhileStatement([NotNull] TinyScriptParser.DoWhileStatementContext context)
        {
            XElement block = new XElement("block", new XAttribute("type", "do_while"));
            Conditional(block, "condition", "statements", context.expression(), context.block());
            return block;
        }

        public override XElement VisitBlock([NotNull] TinyScriptParser.BlockContext context)
        {
            return VisitStatementList(context.statementList());
        }

        private string GetCmpOperatorName(string op)
        {
            switch (op)
            {
                case "==": return "EQ";
                case "!=": return "NEQ";
                case "<": return "LT";
                case "<=": return "LTE";
                case ">": return "GT";
                case ">=": return "GTE";
                default: return "";
            }
        }

        public override XElement VisitForStatement([NotNull] TinyScriptParser.ForStatementContext context)
        {
            XElement block = new XElement("block", new XAttribute("type", "controls_for"));
            XElement field = new XElement("field", new XAttribute("name", "VAR"));
            if (context.varName()[0].GetText() != context.varName()[1].GetText())
            {
                ThrowSyntaxError(context.varName()[1].Start, "Name mismatch");
            }
            string varName = context.varName()[0].GetText();
            if (!variables.ContainsKey(varName))
            {
                ThrowSyntaxError(context.varName()[0].Start, "Variable does not exist");
            }
            if (variables[varName] != VariableType.INT)
            {
                ThrowSyntaxError(context.varName()[0].Start, "Type mismatch");
            }
            field.Add(varName);
            block.Add(field);
            ExpressionElement assignExpr = (ExpressionElement)VisitExpression(context.expression()[0]);
            ExpressionElement compareExpr = (ExpressionElement)VisitExpression(context.expression()[1]);
            if (assignExpr.Type != VariableType.INT || compareExpr.Type != VariableType.INT)
            {
                ThrowSyntaxError(context.expression()[0].Start, "Type mismatch");
            }
            XElement from = new XElement("value", new XAttribute("name", "FROM"));
            from.Add(assignExpr);
            XElement to = new XElement("value", new XAttribute("name", "TO"));
            to.Add(compareExpr);
            block.Add(from);
            block.Add(to);
            XElement incrExpr = VisitIncrementStatement(context.incrementStatement(), varName);
            XElement by = new XElement("value", new XAttribute("name", "BY"));
            by.Add(incrExpr);
            block.Add(by);
            XElement statement = new XElement("statement", new XAttribute("name", "DO"));
            statement.Add(VisitBlock(context.block()));
            block.Add(statement);
            return block;
        }

        private XElement VisitIncrementStatement([NotNull] TinyScriptParser.IncrementStatementContext context, string varName)
        {
            if (context.varName().GetText() != varName)
            {
                ThrowSyntaxError(context.varName().Start, "Name mismatch");
            }
            if (context.expression() == null)
            {
                XElement block = new XElement("block", new XAttribute("type", "math_number"));
                block.Add(new XElement("field", new XAttribute("name", "NUM"), "1"));
                return block;
            }
            ExpressionElement expr = (ExpressionElement)VisitExpression(context.expression());
            if (expr.Type != VariableType.INT)
            {
                ThrowSyntaxError(context.expression().Start, "Type mismatch");
            }
            return expr;
        }

        public override XElement VisitAssignmentStatement([NotNull] TinyScriptParser.AssignmentStatementContext context)
        {
            string varName = context.varName().GetText();
            if (!variables.ContainsKey(varName))
            {
                ThrowSyntaxError(context.varName().Start, "Variable does not exist");
            }
            ExpressionElement expr = (ExpressionElement)VisitExpression(context.expression());
            if (expr.Type != variables[varName])
            {
                ThrowSyntaxError(context.expression().Start, "Type mismatch");
            }
            XElement block = new XElement("block", new XAttribute("type", "variables_set"));
            XElement field = new XElement("field", new XAttribute("name", "VAR"));
            field.Add(varName);
            block.Add(field);
            XElement value = new XElement("value", new XAttribute("name", "VALUE"));
            value.Add(expr);
            block.Add(value);
            return block;
        }

        public override XElement VisitPrintStatement([NotNull] TinyScriptParser.PrintStatementContext context)
        {
            XElement block = new XElement("block", new XAttribute("type", "text_print"));
            XElement value = new XElement("value", new XAttribute("name", "TEXT"));
            ExpressionElement expression = (ExpressionElement)VisitExpression(context.expression());
            if (expression.Type != VariableType.STRING)
            {
                ThrowSyntaxError(context.expression().Start, "Type mismatch");
            }
            value.Add(expression);
            block.Add(value);
            return block;
        }

        public string[] GetVariableNames()
        {
            return variables.Keys.ToArray();
        }

        private bool CheckType(VariableType type1, VariableType type2)
        {
            if (type1 == VariableType.NULL || type2 == VariableType.NULL)
            {
                return true;
            }
            return type1 == type2;
        }
    }
}
