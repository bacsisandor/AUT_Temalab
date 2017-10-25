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
            return VisitStatementList(context.statementList());
        }

        public override XElement VisitStatementList([NotNull] TinyScriptParser.StatementListContext context)
        {
            if (context.ChildCount == 0)
                return null;
            var statements = context.statement();
            XElement root = VisitStatement(statements[0]);
            XElement parent = root;
            for (int i = 1; i < context.ChildCount; i++)
            {
                XElement next = new XElement("next");
                parent.Add(next);
                XElement child = VisitStatement(statements[i]);
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
                case "string": return VariableType.STRING;
                case "boolean": return VariableType.BOOLEAN;
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
            VariableType type = VariableType.NULL;
            if (context.typeName() != null)
            {
                type = GetTypeFromString(context.typeName().GetText());
            }
            ExpressionElement expression = null;
            if (context.expression() != null)
            {
                expression = (ExpressionElement)VisitExpression(context.expression());
                if (type == VariableType.NULL)
                {
                    type = expression.Type;
                }
                else if (type != expression.Type)
                {
                    ThrowSyntaxError(context.expression().Start, "Type mismatch");
                }
            }
            else if (type == VariableType.NULL)
            {
                ThrowSyntaxError(context.Stop, "Assignment missing");
            }
            string varName = context.varName().GetText();
            variables.Add(varName, type);
            XElement block;
            if (context.typeName() == null)
            {
                block = new XElement("block", new XAttribute("type", "create_var"));
            }
            else
            {
                block = new XElement("block", new XAttribute("type", "create_variabletype"));
                XElement typeField = new XElement("field", new XAttribute("name", "type"));
                typeField.Add(GetStringFromType(type));
                block.Add(typeField);
            }
            XElement nameField = new XElement("field", new XAttribute("name", "var"));
            nameField.Add(varName);
            block.Add(nameField);
            if (expression != null)
            {
                XElement value = new XElement("value", new XAttribute("name", "NAME"));
                value.Add(expression);
                block.Add(value);
            }
            return block;
        }

        public override XElement VisitExpression([NotNull] TinyScriptParser.ExpressionContext context)
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
                field = new XElement("field", new XAttribute("name", "var"));
                field.Add(varName);
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

        private XElement Conditional(string type, string condName, string blockName, TinyScriptParser.ConditionContext conditionContext, TinyScriptParser.BlockContext blockContext)
        {
            XElement block = new XElement("block", new XAttribute("type", type));
            XElement value = new XElement("value", new XAttribute("name", condName));
            value.Add(VisitCondition(conditionContext));
            XElement statement = new XElement("statement", new XAttribute("name", blockName));
            statement.Add(VisitBlock(blockContext));
            block.Add(value);
            block.Add(statement);
            return block;
        }

        public override XElement VisitIfStatement([NotNull] TinyScriptParser.IfStatementContext context)
        {
            XElement ifBlock = Conditional("controls_if", "IF0", "DO0", context.condition(), context.block());
            if (context.elseStatement() != null)
            {
                ifBlock.Add(new XElement("mutation", new XAttribute("else", "1")));
                ifBlock.Add(VisitElseStatement(context.elseStatement()));
            }
            return ifBlock;
        }

        public override XElement VisitElseStatement([NotNull] TinyScriptParser.ElseStatementContext context)
        {
            XElement statement = new XElement("statement", new XAttribute("name", "ELSE"));
            statement.Add(VisitBlock(context.block()));
            return statement;
        }

        public override XElement VisitWhileStatement([NotNull] TinyScriptParser.WhileStatementContext context)
        {
            return Conditional("while", "condition", "statements", context.condition(), context.block());
        }

        public override XElement VisitDoWhileStatement([NotNull] TinyScriptParser.DoWhileStatementContext context)
        {
            return Conditional("do_while", "condition", "statements", context.condition(), context.block());
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

        public override XElement VisitCondition([NotNull] TinyScriptParser.ConditionContext context)
        {
            ExpressionElement leftExpr = (ExpressionElement)VisitExpression(context.expression()[0]);
            ExpressionElement rightExpr = (ExpressionElement)VisitExpression(context.expression()[1]);
            if (leftExpr.Type != VariableType.INT || rightExpr.Type != VariableType.INT)
            {
                ThrowSyntaxError(context.COND().Symbol, "Type mismatch");
            }
            XElement block = new XElement("block", new XAttribute("type", "logic_compare"));
            XElement field = new XElement("field", new XAttribute("name", "OP"));
            field.Value = GetCmpOperatorName(context.COND().GetText());
            block.Add(field);
            XElement left = new XElement("value", new XAttribute("name", "A"));
            left.Add(leftExpr);
            XElement right = new XElement("value", new XAttribute("name", "B"));
            right.Add(rightExpr);
            block.Add(left);
            block.Add(right);
            return block;
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

        public XElement VisitIncrementStatement([NotNull] TinyScriptParser.IncrementStatementContext context, string varName)
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
            XElement textBlock = new XElement("block", new XAttribute("type", "text"));
            textBlock.Add(new XElement("field", new XAttribute("name", "TEXT"), context.STRING().GetText().Trim('"')));
            value.Add(textBlock);
            block.Add(value);
            return block;
        }

        public string[] GetVariableNames()
        {
            return variables.Keys.ToArray();
        }
    }
}
