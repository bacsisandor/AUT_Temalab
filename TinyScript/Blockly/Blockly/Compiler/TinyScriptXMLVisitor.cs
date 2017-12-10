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
    public class TinyScriptXMLVisitor : TinyScriptBaseVisitor<XElement>
    {
        private TinyScriptVisitor.ITypeData typeData;

        public TinyScriptXMLVisitor(TinyScriptVisitor.ITypeData typeData)
        {
            this.typeData = typeData;
        }

        public override XElement VisitProgram([NotNull] TinyScriptParser.ProgramContext context)
        {
            XElement xml = new XElement("xml");
            xml.Add(VisitVariableDeclarationList(context.variableDeclarationList()));
            XElement statements = VisitStatementList(context.statementList());
            if (statements != null)
            {
                statements.Add(new XAttribute("x", "0"), new XAttribute("y", "100"));
                xml.Add(statements);
            }
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

        public override XElement VisitVariableDeclaration([NotNull] TinyScriptParser.VariableDeclarationContext context)
        {
            return Visit(context.GetChild(0));
        }

        public override XElement VisitVariableDeclaration1([NotNull] TinyScriptParser.VariableDeclaration1Context context)
        {
            XElement expression = null;
            if (context.expression() != null)
            {
                expression = VisitExpression(context.expression());
            }
            XElement block = MakeVariable(context.varName().Start, false);
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
            XElement expression = VisitExpression(context.expression());
            XElement block = MakeVariable(context.varName().Start, true);
            XElement value = new XElement("value", new XAttribute("name", "NAME"));
            value.Add(expression);
            block.Add(value);
            return block;
        }

        private XElement MakeVariable(IToken name, bool var)
        {
            string varName = name.Text;
            VariableType type = typeData.GetVariableType(varName);
            XElement block = new XElement("block", new XAttribute("type", var ? "create_var" : "create_variabletype"));
            if (!var)
            {
                XElement typeField = new XElement("field", new XAttribute("name", "type"));
                typeField.Add(type.ToString());
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
            XElement leftExpr = VisitSum(context.sum()[0]);
            XElement rightExpr = VisitSum(context.sum()[1]);
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
            return block;
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
            XElement leftExpr = VisitProduct(products.ElementAt(0));
            left.Add(leftExpr);
            block.Add(left);
            XElement right = new XElement("value", new XAttribute("name", "B"));
            XElement rightExpr = Expression(ops.Skip(1), products.Skip(1));
            right.Add(rightExpr);
            block.Add(right);
            return block;
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
            XElement leftExpr = VisitSignedArgument(args.ElementAt(0));
            left.Add(leftExpr);
            block.Add(left);
            XElement right = new XElement("value", new XAttribute("name", "B"));
            XElement rightExpr = Product(ops.Skip(1), args.Skip(1));
            right.Add(rightExpr);
            block.Add(right);
            return block;
        }

        public override XElement VisitSignedArgument([NotNull] TinyScriptParser.SignedArgumentContext context)
        {
            ITerminalNode op = context.PLUSMINUS();
            if (op == null || op.GetText() == "+")
            {
                return VisitArgument(context.argument());
            }
            return Negate(context.argument().Start, VisitArgument(context.argument()));
        }

        private XElement Negate(IToken token, XElement expression)
        {
            XElement block = new XElement("block", new XAttribute("type", "math_arithmetic"));
            XElement field = new XElement("field", new XAttribute("name", "OP"), "MINUS");
            block.Add(field);
            XElement left = new XElement("value", new XAttribute("name", "A"));
            XElement zero = new XElement("block", new XAttribute("type", "math_number"));
            XElement zeroField = new XElement("field", new XAttribute("name", "NUM"));
            zeroField.Add("0");
            zero.Add(zeroField);
            left.Add(zero);
            block.Add(left);
            XElement right = new XElement("value", new XAttribute("name", "B"));
            right.Add(expression);
            block.Add(right);
            return block;
        }

        public override XElement VisitArgument([NotNull] TinyScriptParser.ArgumentContext context)
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

        public override XElement VisitVarName([NotNull] TinyScriptParser.VarNameContext context)
        {
            string varName = context.GetText();
            XElement block = new XElement("block", new XAttribute("type", "variables_get"));
            XElement field = new XElement("field", new XAttribute("name", "VAR"));
            field.Add(varName);
            block.Add(field);
            return block;
        }

        public override XElement VisitValue([NotNull] TinyScriptParser.ValueContext context)
        {
            XElement block;
            XElement field;
            if (context.INT() != null)
            {
                block = new XElement("block", new XAttribute("type", "math_number"));
                field = new XElement("field", new XAttribute("name", "NUM"));
                field.Add(context.GetText());
                block.Add(field);
                return block;
            }
            if (context.BOOLEAN() != null)
            {
                block = new XElement("block", new XAttribute("type", "logic_boolean"));
                field = new XElement("field", new XAttribute("name", "BOOL"));
                field.Add(context.GetText().ToUpper());
                block.Add(field);
                return block;
            }
            block = new XElement("block", new XAttribute("type", "text"));
            field = new XElement("field", new XAttribute("name", "TEXT"));
            field.Add(context.GetText().Trim('"'));
            block.Add(field);
            return block;
        }

        private void Conditional(XElement block, string condName, string blockName, TinyScriptParser.ExpressionContext expressionContext, TinyScriptParser.BlockContext blockContext)
        {
            XElement value = new XElement("value", new XAttribute("name", condName));
            XElement expression = VisitExpression(expressionContext);
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
            XElement block = new XElement("block", new XAttribute("type", "for"));
            XElement field = new XElement("field", new XAttribute("name", "variable1"));
            string varName = context.varName().GetText();
            field.Add(varName);
            block.Add(field);
            XElement assignExpr = VisitExpression(context.expression()[0]);
            XElement compareExpr = VisitExpression(context.expression()[1]);
            XElement from = new XElement("value", new XAttribute("name", "init"));
            from.Add(assignExpr);
            XElement to = new XElement("value", new XAttribute("name", "condition"));
            to.Add(compareExpr);
            block.Add(from);
            block.Add(to);
            XElement field2 = new XElement("field", new XAttribute("name", "variable2"));
            field2.Add(context.incrementation().varName().GetText());
            block.Add(field2);
            XElement dirField = new XElement("field", new XAttribute("name", "direction"));
            bool direction;
            if (context.incrementation().INCDEC1() != null)
            {
                direction = context.incrementation().INCDEC1().GetText() == "++";
            }
            else
            {
                direction = context.incrementation().INCDEC2().GetText() == "+=";
            }
            dirField.Add(direction ? "increment" : "decrement");
            block.Add(dirField);
            XElement incrExpr = Increment(context.incrementation().varName(), context.incrementation().expression());
            XElement by = new XElement("value", new XAttribute("name", "inc_number"));
            by.Add(incrExpr);
            block.Add(by);
            XElement statement = new XElement("statement", new XAttribute("name", "core"));
            statement.Add(VisitBlock(context.block()));
            block.Add(statement);
            return block;
        }

        private XElement Increment([NotNull] TinyScriptParser.VarNameContext varContext, TinyScriptParser.ExpressionContext exprContext)
        {
            if (exprContext == null)
            {
                XElement block = new XElement("block", new XAttribute("type", "math_number"));
                block.Add(new XElement("field", new XAttribute("name", "NUM"), "1"));
                return block;
            }
            return VisitExpression(exprContext);
        }

        public override XElement VisitIncrementStatement([NotNull] TinyScriptParser.IncrementStatementContext context)
        {
            TinyScriptParser.IncrementationContext incContext = context.incrementation();
            XElement block = new XElement("block", new XAttribute("type", "math_change"));
            string varName = incContext.varName().GetText();
            XElement field = new XElement("field", new XAttribute("name", "VAR"));
            field.Add(varName);
            block.Add(field);
            XElement value = new XElement("value", new XAttribute("name", "DELTA"));
            if (incContext.expression() == null)
            {
                XElement num = new XElement("block", new XAttribute("type", "math_number"));
                num.Add(new XElement("field", new XAttribute("name", "NUM"), incContext.INCDEC1().GetText() == "++" ? "1" : "-1"));
                value.Add(num);
            }
            else
            {
                XElement expression = VisitExpression(incContext.expression());
                if (incContext.INCDEC2().GetText() == "-=")
                {
                    expression = Negate(incContext.expression().Start, expression);
                }
                value.Add(expression);
            }
            block.Add(value);
            return block;
        }

        public override XElement VisitAssignmentStatement([NotNull] TinyScriptParser.AssignmentStatementContext context)
        {
            string varName = context.varName().GetText();
            XElement expr = VisitExpression(context.expression());
            XElement block = new XElement("block", new XAttribute("type", "variables_set"));
            XElement field = new XElement("field", new XAttribute("name", "VAR"));
            field.Add(varName);
            block.Add(field);
            XElement value = new XElement("value", new XAttribute("name", "VALUE"));
            value.Add(expr);
            block.Add(value);
            return block;
        }

        public override XElement VisitFunctionCallStatement([NotNull] TinyScriptParser.FunctionCallStatementContext context)
        {
            return VisitFunctionCall(context.functionCall());
        }

        public override XElement VisitFunctionCall([NotNull] TinyScriptParser.FunctionCallContext context)
        {
            string functionName = context.functionName().GetText();
            IToken nameToken = context.functionName().Start;
            switch (functionName)
            {
                case "print": return PrintFunction(nameToken, context.expression());
                case "abs": return AbsFunction(nameToken, context.expression());
                case "min": return MinMaxFunction(nameToken, context.expression(), false);
                case "max": return MinMaxFunction(nameToken, context.expression(), true);
                default: return new XElement("test");
            }
        }

        private XElement PrintFunction(IToken nameToken, TinyScriptParser.ExpressionContext[] args)
        {
            XElement block = new XElement("block", new XAttribute("type", "text_print"));
            XElement value = new XElement("value", new XAttribute("name", "TEXT"));
            XElement expression = VisitExpression(args[0]);
            value.Add(expression);
            block.Add(value);
            return block;
        }

        private XElement AbsFunction(IToken nameToken, TinyScriptParser.ExpressionContext[] args)
        {
            XElement block = new XElement("block", new XAttribute("type", "abs"));
            XElement value = new XElement("value", new XAttribute("name", "NAME"));
            XElement expression = VisitExpression(args[0]);
            value.Add(expression);
            block.Add(value);
            return block;
        }

        private XElement MinMaxFunction(IToken nameToken, TinyScriptParser.ExpressionContext[] args, bool max)
        {
            XElement block = new XElement("block", new XAttribute("type", "maximum_select"));
            XElement field = new XElement("field", new XAttribute("name", "select"));
            field.Add(max ? "max" : "min");
            block.Add(field);
            XElement mutation = new XElement("mutation", new XAttribute("items", args.Length));
            block.Add(mutation);
            for (int i = 0; i < args.Length; i++)
            {
                XElement value = new XElement("value", new XAttribute("name", $"ADD{ i }"));
                XElement expr = VisitExpression(args[i]);
                value.Add(expr);
                block.Add(value);
            }
            return block;
        }

        public override XElement VisitArrayDeclaration([NotNull] TinyScriptParser.ArrayDeclarationContext context)
        {
            string varName = context.varName().GetText();
            XElement block = new XElement("block", new XAttribute("type", "create_array"));
            XElement typeField = new XElement("field", new XAttribute("name", "type"));
            VariableType type = typeData.GetVariableType(varName);
            typeField.Add(type.ElementType);
            block.Add(typeField);
            XElement nameField = new XElement("field", new XAttribute("name", "name"));
            nameField.Add(varName);
            block.Add(nameField);
            XElement size = new XElement("value", new XAttribute("name", "size"));
            XElement expr = VisitExpression(context.expression());
            size.Add(expr);
            block.Add(size);
            return block;
        }

        public override XElement VisitArrayAssignmentStatement([NotNull] TinyScriptParser.ArrayAssignmentStatementContext context)
        {
            XElement block = new XElement("block", new XAttribute("type", "set_array"));
            XElement nameField = new XElement("field", new XAttribute("name", "item"));
            string varName = context.varName().GetText();
            nameField.Add(varName);
            XElement index = new XElement("value", new XAttribute("name", "index"));
            XElement idxExpression = VisitExpression(context.expression()[0]);
            index.Add(idxExpression);
            XElement value = new XElement("value", new XAttribute("name", "value"));
            XElement valueExpression = VisitExpression(context.expression()[1]);
            value.Add(valueExpression);
            block.Add(nameField);
            block.Add(index);
            block.Add(value);
            return block;
        }

        public override XElement VisitIndexedArray([NotNull] TinyScriptParser.IndexedArrayContext context)
        {
            XElement block = new XElement("block", new XAttribute("type", "get_array"));
            XElement nameField = new XElement("field", new XAttribute("name", "variable"));
            string varName = context.varName().GetText();
            nameField.Add(varName);
            XElement index = new XElement("value", new XAttribute("name", "index"));
            XElement idxExpression = VisitExpression(context.expression());
            index.Add(idxExpression);
            block.Add(nameField);
            block.Add(index);
            return block;
        }

        public override XElement VisitArrayInitialization([NotNull] TinyScriptParser.ArrayInitializationContext context)
        {
            XElement block = new XElement("block", new XAttribute("type", "init_array"));
            var expressions = context.expression();
            XElement mutation = new XElement("mutation", new XAttribute("items", expressions.Length));
            block.Add(mutation);
            XElement typeField = new XElement("field", new XAttribute("name", "type"));
            VariableType type = VariableType.ArrayFromString(context.typeName().GetText(), expressions.Length);
            typeField.Add(type.ElementType.Name);
            block.Add(typeField);
            string varName = context.varName().GetText();
            XElement nameField = new XElement("field", new XAttribute("name", "name"));
            nameField.Add(varName);
            block.Add(nameField);
            for (int i = 0; i < expressions.Length; i++)
            {
                XElement value = new XElement("value", new XAttribute("name", $"ADD{ i }"));
                XElement expression = VisitExpression(expressions[i]);
                value.Add(expression);
                block.Add(value);
            }
            return block;
        }

        public override XElement VisitReadStatement([NotNull] TinyScriptParser.ReadStatementContext context)
        {
            XElement block = new XElement("block", new XAttribute("type", "read_input"));
            XElement value = new XElement("value", new XAttribute("name", "NAME"));
            value.Add(VisitVarName(context.varName()));
            block.Add(value);
            return block;
        }

        public override XElement VisitCountStatement([NotNull] TinyScriptParser.CountStatementContext context)
        {
            XElement block = new XElement("block", new XAttribute("type", "count"));
            XElement field = new XElement("field", new XAttribute("name", "variable"));
            string varName = context.varName()[0].GetText();
            field.Add(varName);
            block.Add(field);
            XElement initField = new XElement("field", new XAttribute("name", "initial"));
            string from = context.@int()[0].GetText();
            initField.Add(from);
            block.Add(initField);
            XElement untilField = new XElement("field", new XAttribute("name", "until"));
            string to = context.@int()[1].GetText();
            untilField.Add(to);
            block.Add(untilField);
            XElement dirField = new XElement("field", new XAttribute("name", "direction"));
            bool direction;
            bool incrOne = (context.incrementation().INCDEC1() != null);
            if (incrOne)
            {
                direction = context.incrementation().INCDEC1().GetText() == "++";
            }
            else
            {
                direction = context.incrementation().INCDEC2().GetText() == "+=";
            }
            dirField.Add(direction ? "increment" : "decrement");
            block.Add(dirField);
            XElement incrField = new XElement("field", new XAttribute("name", "inc_value"));
            string increment;
            if (incrOne)
            {
                increment = "1";
            }
            else
            {
                increment = context.incrementation().expression().sum()[0].product()[0].signedArgument()[0].argument().value().GetText();
                if (context.incrementation().expression().sum()[0].product()[0].signedArgument()[0].PLUSMINUS() != null)
                {
                    increment = context.incrementation().expression().sum()[0].product()[0].signedArgument()[0].PLUSMINUS().GetText() + increment;
                }
            }
            incrField.Add(increment);
            block.Add(incrField);
            XElement statement = new XElement("statement", new XAttribute("name", "core"));
            statement.Add(VisitBlock(context.block()));
            block.Add(statement);
            return block;
        }
    }
}
