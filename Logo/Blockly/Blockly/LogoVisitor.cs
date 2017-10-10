using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Antlr4.Runtime.Misc;
using System.Xml.Linq;
using Antlr4.Runtime.Tree;

namespace Blockly
{
    public class LogoVisitor : logoBaseVisitor<XElement>
    {
        public override XElement VisitProgram([NotNull] logoParser.ProgramContext context)
        {
            return VisitStatementList(context.statementList());
        }

        public override XElement VisitStatementList([NotNull] logoParser.StatementListContext context)
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

        public override XElement VisitStatement([NotNull] logoParser.StatementContext context)
        {
            return Visit(context.GetChild(0));
        }

        public override XElement VisitExpr([NotNull] logoParser.ExprContext context)
        {
            return Expr(context.ADDSUB(), context.product());
        }

        private XElement Expr(IEnumerable<ITerminalNode> ops, IEnumerable<logoParser.ProductContext> products)
        {
            if (ops.Count() == 0)
            {
                return VisitProduct(products.ElementAt(0));
            }
            XElement block = new XElement("block", new XAttribute("type", "operation"));
            XElement field = new XElement("field", new XAttribute("name", "op"));
            field.Value = ops.ElementAt(0).GetText() == "+" ? "add" : "substract";
            block.Add(field);
            XElement left = new XElement("value", new XAttribute("name", "left_value"));
            left.Add(VisitProduct(products.ElementAt(0)));
            block.Add(left);
            XElement right = new XElement("value", new XAttribute("name", "right_value"));
            right.Add(Expr(ops.Skip(1), products.Skip(1)));
            block.Add(right);
            return block;
        }

        public override XElement VisitProduct([NotNull] logoParser.ProductContext context)
        {
            return Product(context.MULDIV(), context.signedArgument());
        }

        private XElement Product(IEnumerable<ITerminalNode> ops, IEnumerable<logoParser.SignedArgumentContext> arguments)
        {
            if (ops.Count() == 0)
            {
                return VisitSignedArgument(arguments.ElementAt(0));
            }
            XElement block = new XElement("block", new XAttribute("type", "operation"));
            XElement field = new XElement("field", new XAttribute("name", "op"));
            field.Value = ops.ElementAt(0).GetText() == "*" ? "multiply" : "divide";
            block.Add(field);
            XElement left = new XElement("value", new XAttribute("name", "left_value"));
            left.Add(VisitSignedArgument(arguments.ElementAt(0)));
            block.Add(left);
            XElement right = new XElement("value", new XAttribute("name", "right_value"));
            right.Add(Product(ops.Skip(1), arguments.Skip(1)));
            block.Add(right);
            return block;
        }

        public override XElement VisitSignedArgument([NotNull] logoParser.SignedArgumentContext context)
        {
            ITerminalNode op = context.ADDSUB();
            if (op == null || op.GetText() == "+")
            {
                return VisitArgument(context.argument());
            }
            XElement block = new XElement("block", new XAttribute("type", "operation"));
            XElement field = new XElement("field", new XAttribute("name", "op"));
            field.Value = "substract";
            block.Add(field);
            XElement left = new XElement("value", new XAttribute("name", "left_value"));
            XElement zero = new XElement("block", new XAttribute("type", "math_number"));
            XElement zeroField = new XElement("field", new XAttribute("name", "NUM"));
            zeroField.Add("0");
            zero.Add(zeroField);
            left.Add(zero);
            block.Add(left);
            XElement right = new XElement("value", new XAttribute("name", "right_value"));
            right.Add(VisitArgument(context.argument()));
            block.Add(right);
            return block;
        }

        public override XElement VisitArgument([NotNull] logoParser.ArgumentContext context)
        {
            if (context.INT() == null)
            {
                return VisitExpr(context.expr());
            }
            XElement block = new XElement("block", new XAttribute("type", "math_number"));
            XElement field = new XElement("field", new XAttribute("name", "NUM"));
            field.Add(context.INT().GetText());
            block.Add(field);
            return block;
        }

        private XElement Statement(string name, string[] exprNames, logoParser.ExprContext[] exprs)
        {
            XElement block = new XElement("block", new XAttribute("type", name));
            for (int i = 0; i < exprs.Length; i++)
            {
                XElement value = new XElement("value", new XAttribute("name", exprNames[i]));
                block.Add(value);
                value.Add(VisitExpr(exprs[i]));
            }
            return block;
        }

        public override XElement VisitForward([NotNull] logoParser.ForwardContext context)
        {
            return Statement("move_forward", new string[] { "forward_pixels" }, new logoParser.ExprContext[] { context.expr() });
        }

        public override XElement VisitBack([NotNull] logoParser.BackContext context)
        {
            return Statement("move_backward", new string[] { "backward_pixels" }, new logoParser.ExprContext[] { context.expr() });
        }

        public override XElement VisitLeft([NotNull] logoParser.LeftContext context)
        {
            return Statement("turn_left", new string[] { "turn_left" }, new logoParser.ExprContext[] { context.expr() });
        }

        public override XElement VisitRight([NotNull] logoParser.RightContext context)
        {
            return Statement("turn_right", new string[] { "turn_right" }, new logoParser.ExprContext[] { context.expr() });
        }

        public override XElement VisitSetxy([NotNull] logoParser.SetxyContext context)
        {
            return Statement("set_xy", new string[] { "x_position", "y_position" }, context.expr());
        }

        public override XElement VisitSetx([NotNull] logoParser.SetxContext context)
        {
            return Statement("set_x", new string[] { "x_position" }, new logoParser.ExprContext[] { context.expr() });
        }

        public override XElement VisitSety([NotNull] logoParser.SetyContext context)
        {
            return Statement("set_y", new string[] { "y_position" }, new logoParser.ExprContext[] { context.expr() });
        }

        public override XElement VisitSetheading([NotNull] logoParser.SetheadingContext context)
        {
            return Statement("set_heading", new string[] { "head_position" }, new logoParser.ExprContext[] { context.expr() });
        }

        public override XElement VisitHome([NotNull] logoParser.HomeContext context)
        {
            return Statement("home", new string[0], new logoParser.ExprContext[0]);
        }

        public override XElement VisitClean([NotNull] logoParser.CleanContext context)
        {
            return Statement("clean", new string[0], new logoParser.ExprContext[0]);
        }

        public override XElement VisitClearscreen([NotNull] logoParser.ClearscreenContext context)
        {
            return Statement("clear_screen", new string[0], new logoParser.ExprContext[0]);
        }

        public override XElement VisitPendown([NotNull] logoParser.PendownContext context)
        {
            return Statement("pen_down", new string[0], new logoParser.ExprContext[0]);
        }

        public override XElement VisitPenup([NotNull] logoParser.PenupContext context)
        {
            return Statement("pen_up", new string[0], new logoParser.ExprContext[0]);
        }

        public override XElement VisitRepeat([NotNull] logoParser.RepeatContext context)
        {
            XElement block = new XElement("block", new XAttribute("type", "repeat"));
            XElement field = new XElement("field", new XAttribute("name", "repeat_number"));
            field.Add(context.INT().GetText());
            block.Add(field);
            XElement statement = new XElement("statement", new XAttribute("name", "repeat"));
            statement.Add(VisitStatementList(context.statementList()));
            block.Add(statement);
            return block;
        }
    }
}
