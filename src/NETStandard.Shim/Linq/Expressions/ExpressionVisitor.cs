#if NET35
using System.Collections.Generic;
using System.Collections.ObjectModel;


namespace System.Linq.Expressions
{
    /// <summary>
    /// Represents a visitor or rewriter for expression trees.
    /// </summary>
    /// <remarks>
    /// This class is designed to be inherited to create more specialized
    /// classes whose functionality requires traversing, examining or copying
    /// an expression tree.
    /// </remarks>
    public abstract class ExpressionVisitor
    {
        /// <summary>
        /// Initializes a new instance of <see cref="ExpressionVisitor"/>.
        /// </summary>
        protected ExpressionVisitor() { }

        /// <summary>
        /// Dispatches the expression to one of the more specialized visit methods in this class.
        /// </summary>
        /// <param name="node">
        /// The expression to visit.
        /// </param>
        /// <returns>
        /// The modified expression, if it or any subexpression was modified;
        /// otherwise, returns the original expression.
        /// </returns>
        public virtual Expression Visit(Expression node)
        {
            if (node == null)
            {
                return node;
            }
            switch (node.NodeType)
            {
                case ExpressionType.Negate:
                case ExpressionType.NegateChecked:
                case ExpressionType.Not:
                case ExpressionType.Convert:
                case ExpressionType.ConvertChecked:
                case ExpressionType.ArrayLength:
                case ExpressionType.Quote:
                case ExpressionType.TypeAs:
                    return this.VisitUnary((UnaryExpression)node);
                case ExpressionType.Add:
                case ExpressionType.AddChecked:
                case ExpressionType.Subtract:
                case ExpressionType.SubtractChecked:
                case ExpressionType.Multiply:
                case ExpressionType.MultiplyChecked:
                case ExpressionType.Divide:
                case ExpressionType.Modulo:
                case ExpressionType.And:
                case ExpressionType.AndAlso:
                case ExpressionType.Or:
                case ExpressionType.OrElse:
                case ExpressionType.LessThan:
                case ExpressionType.LessThanOrEqual:
                case ExpressionType.GreaterThan:
                case ExpressionType.GreaterThanOrEqual:
                case ExpressionType.Equal:
                case ExpressionType.NotEqual:
                case ExpressionType.Coalesce:
                case ExpressionType.ArrayIndex:
                case ExpressionType.RightShift:
                case ExpressionType.LeftShift:
                case ExpressionType.ExclusiveOr:
                    return this.VisitBinary((BinaryExpression)node);
                case ExpressionType.TypeIs:
                    return this.VisitTypeIs((TypeBinaryExpression)node);
                case ExpressionType.Conditional:
                    return this.VisitConditional((ConditionalExpression)node);
                case ExpressionType.Constant:
                    return this.VisitConstant((ConstantExpression)node);
                case ExpressionType.Parameter:
                    return this.VisitParameter((ParameterExpression)node);
                case ExpressionType.MemberAccess:
                    return this.VisitMemberAccess((MemberExpression)node);
                case ExpressionType.Call:
                    return this.VisitMethodCall((MethodCallExpression)node);
                case ExpressionType.Lambda:
                    return this.VisitLambda((LambdaExpression)node);
                case ExpressionType.New:
                    return this.VisitNew((NewExpression)node);
                case ExpressionType.NewArrayInit:
                case ExpressionType.NewArrayBounds:
                    return this.VisitNewArray((NewArrayExpression)node);
                case ExpressionType.Invoke:
                    return this.VisitInvocation((InvocationExpression)node);
                case ExpressionType.MemberInit:
                    return this.VisitMemberInit((MemberInitExpression)node);
                case ExpressionType.ListInit:
                    return this.VisitListInit((ListInitExpression)node);
                default:
                    throw new InvalidOperationException(string.Format("Unhandled expression type: '{0}'", node.NodeType));
            }
        }

        /// <summary>
        /// Visits the children of the <see cref="MemberBinding" />.
        /// </summary>
        /// <param name="node">
        /// The expression to visit.
        /// </param>
        /// <returns>The modified expression, if it or any subexpression was modified;
        /// otherwise, returns the original expression.
        /// </returns>
        protected virtual MemberBinding VisitBinding(MemberBinding node)
        {
            switch (node.BindingType)
            {
                case MemberBindingType.Assignment:
                    return this.VisitMemberAssignment((MemberAssignment)node);
                case MemberBindingType.MemberBinding:
                    return this.VisitMemberMemberBinding((MemberMemberBinding)node);
                case MemberBindingType.ListBinding:
                    return this.VisitMemberListBinding((MemberListBinding)node);
                default:
                    throw new InvalidOperationException(string.Format("Unhandled binding type '{0}'", node.BindingType));
            }
        }

        /// <summary>
        /// Visits the children of the <see cref="ElementInit" />.
        /// </summary>
        /// <param name="node">
        /// The expression to visit.
        /// </param>
        /// <returns>The modified expression, if it or any subexpression was modified;
        /// otherwise, returns the original expression.
        /// </returns>
        protected virtual ElementInit VisitElementInitializer(ElementInit node)
        {
            var arguments = this.VisitExpressionList(node.Arguments);
            return arguments != node.Arguments ? Expression.ElementInit(node.AddMethod, arguments) : node;
        }

        /// <summary>
        /// Visits the children of the <see cref="UnaryExpression" />.
        /// </summary>
        /// <param name="node">
        /// The expression to visit.
        /// </param>
        /// <returns>
        /// The modified expression, if it or any subexpression was modified;
        /// otherwise, returns the original expression.
        /// </returns>
        protected virtual Expression VisitUnary(UnaryExpression node)
        {
            var operand = this.Visit(node.Operand);
            return operand != node.Operand ? Expression.MakeUnary(node.NodeType, operand, node.Type, node.Method) : node;
        }

        /// <summary>
        /// Visits the children of the <see cref="BinaryExpression" />.
        /// </summary>
        /// <param name="node">
        /// The expression to visit.
        /// </param>
        /// <returns>
        /// The modified expression, if it or any subexpression was modified;
        /// otherwise, returns the original expression.
        /// </returns>
        protected virtual Expression VisitBinary(BinaryExpression node)
        {
            var left = this.Visit(node.Left);
            var right = this.Visit(node.Right);
            var conversion = this.Visit(node.Conversion);
            if (left != node.Left || right != node.Right || conversion != node.Conversion)
            {
                if (node.NodeType == ExpressionType.Coalesce && node.Conversion != null)
                {
                    return Expression.Coalesce(left, right, conversion as LambdaExpression);
                }
                return Expression.MakeBinary(node.NodeType, left, right, node.IsLiftedToNull, node.Method);
            }
            return node;
        }

        protected virtual Expression VisitTypeIs(TypeBinaryExpression b)
        {
            var expr = this.Visit(b.Expression);
            return expr != b.Expression ? Expression.TypeIs(expr, b.TypeOperand) : b;
        }

        /// <summary>
        /// Visits the <see cref="ConstantExpression" />.
        /// </summary>
        /// <param name="node">The expression to visit.</param>
        /// <returns>
        /// The modified expression, if it or any subexpression was modified;
        /// otherwise, returns the original expression.
        /// </returns>
        protected virtual Expression VisitConstant(ConstantExpression node) => node;

        /// <summary>
        /// Visits the children of the <see cref="ConditionalExpression" />.
        /// </summary>
        /// <param name="node">
        /// The expression to visit.
        /// </param>
        /// <returns>
        /// The modified expression, if it or any subexpression was modified;
        /// otherwise, returns the original expression.
        /// </returns>
        protected virtual Expression VisitConditional(ConditionalExpression node)
        {
            var test = this.Visit(node.Test);
            var ifTrue = this.Visit(node.IfTrue);
            var ifFalse = this.Visit(node.IfFalse);
            if (test != node.Test || ifTrue != node.IfTrue || ifFalse != node.IfFalse)
            {
                return Expression.Condition(test, ifTrue, ifFalse);
            }
            return node;
        }

        /// <summary>
        /// Visits the <see cref="ParameterExpression" />.
        /// </summary>
        /// <param name="node">The expression to visit.</param>
        /// <returns>
        /// The modified expression, if it or any subexpression was modified;
        /// otherwise, returns the original expression.
        /// </returns>
        protected virtual Expression VisitParameter(ParameterExpression node)
        {
            return node;
        }

        protected virtual Expression VisitMemberAccess(MemberExpression m)
        {
            var exp = this.Visit(m.Expression);
            return exp != m.Expression ? Expression.MakeMemberAccess(exp, m.Member) : m;
        }

        /// <summary>
        /// Visits the children of the <see cref="MethodCallExpression" />.
        /// </summary>
        /// <param name="node">
        /// The expression to visit.
        /// </param>
        /// <returns>
        /// The modified expression, if it or any subexpression was modified;
        /// otherwise, returns the original expression.
        /// </returns>
        protected virtual Expression VisitMethodCall(MethodCallExpression node)
        {
            var obj = this.Visit(node.Object);
            IEnumerable<Expression> args = this.VisitExpressionList(node.Arguments);
            if (obj != node.Object || args != node.Arguments)
            {
                return Expression.Call(obj, node.Method, args);
            }
            return node;
        }

        /// <summary>
        /// Dispatches the list of expressions to one of the more specialized visit methods in this class.
        /// </summary>
        /// <param name="nodes">
        /// The expressions to visit.
        /// </param>
        /// <returns>
        /// The modified expression list, if any of the elements were modified;
        /// otherwise, returns the original expression list.
        /// </returns>
        protected virtual ReadOnlyCollection<Expression> VisitExpressionList(ReadOnlyCollection<Expression> nodes)
        {
            List<Expression> list = null;
            for (int i = 0, n = nodes.Count; i < n; i++)
            {
                var p = this.Visit(nodes[i]);
                if (list != null)
                {
                    list.Add(p);
                }
                else if (p != nodes[i])
                {
                    list = new List<Expression>(n);
                    for (var j = 0; j < i; j++)
                    {
                        list.Add(nodes[j]);
                    }
                    list.Add(p);
                }
            }
            return list != null ? list.AsReadOnly() : nodes;
        }

        /// <summary>
        /// Visits the children of the <see cref="MemberAssignment" />.
        /// </summary>
        /// <param name="node">
        /// The expression to visit.
        /// </param>
        /// <returns>The modified expression, if it or any subexpression was modified;
        /// otherwise, returns the original expression.
        /// </returns>
        protected virtual MemberAssignment VisitMemberAssignment(MemberAssignment node)
        {
            var e = this.Visit(node.Expression);
            return e != node.Expression ? Expression.Bind(node.Member, e) : node;
        }

        /// <summary>
        /// Visits the children of the <see cref="MemberMemberBinding" />.
        /// </summary>
        /// <param name="node">
        /// The expression to visit.
        /// </param>
        /// <returns>The modified expression, if it or any subexpression was modified;
        /// otherwise, returns the original expression.
        /// </returns>
        protected virtual MemberMemberBinding VisitMemberMemberBinding(MemberMemberBinding node)
        {
            var bindings = this.VisitBindingList(node.Bindings);
            return bindings != node.Bindings ? Expression.MemberBind(node.Member, bindings) : node;
        }

        /// <summary>
        /// Visits the children of the <see cref="MemberListBinding" />.
        /// </summary>
        /// <param name="node">
        /// The expression to visit.
        /// </param>
        /// <returns>
        /// The modified expression, if it or any subexpression was modified;
        /// otherwise, returns the original expression.
        /// </returns>
        protected virtual MemberListBinding VisitMemberListBinding(MemberListBinding node)
        {
            var initializers = this.VisitElementInitializerList(node.Initializers);
            return initializers != node.Initializers ? Expression.ListBind(node.Member, initializers) : node;
        }

        protected virtual IEnumerable<MemberBinding> VisitBindingList(ReadOnlyCollection<MemberBinding> original)
        {
            List<MemberBinding> list = null;
            for (int i = 0, n = original.Count; i < n; i++)
            {
                var b = this.VisitBinding(original[i]);
                if (list != null)
                {
                    list.Add(b);
                }
                else if (b != original[i])
                {
                    list = new List<MemberBinding>(n);
                    for (var j = 0; j < i; j++)
                    {
                        list.Add(original[j]);
                    }
                    list.Add(b);
                }
            }
            if (list != null)
            {
                return list;
            }
            return original;
        }

        protected virtual IEnumerable<ElementInit> VisitElementInitializerList(ReadOnlyCollection<ElementInit> original)
        {
            List<ElementInit> list = null;
            for (int i = 0, n = original.Count; i < n; i++)
            {
                var init = this.VisitElementInitializer(original[i]);
                if (list != null)
                {
                    list.Add(init);
                }
                else if (init != original[i])
                {
                    list = new List<ElementInit>(n);
                    for (var j = 0; j < i; j++)
                    {
                        list.Add(original[j]);
                    }
                    list.Add(init);
                }
            }
            return list != null 
                ? (IEnumerable<ElementInit>) list 
                : original;
        }

        /// <summary>
        /// Visits the children of the <see cref="LambdaExpression" />.
        /// </summary>
        /// <param name="node">
        /// The expression to visit.
        /// </param>
        /// <returns>
        /// The modified expression, if it or any subexpression was modified;
        /// otherwise, returns the original expression.
        /// </returns>
        protected virtual Expression VisitLambda(LambdaExpression node)
        {
            var body = this.Visit(node.Body);
            return body != node.Body 
                ? Expression.Lambda(node.Type, body, node.Parameters) 
                : node;
        }

        /// <summary>
        /// Visits the children of the <see cref="NewExpression" />.
        /// </summary>
        /// <param name="node">
        /// The expression to visit.
        /// </param>
        /// <returns>
        /// The modified expression, if it or any subexpression was modified;
        /// otherwise, returns the original expression.
        /// </returns>
        protected virtual NewExpression VisitNew(NewExpression node)
        {
            IEnumerable<Expression> args = this.VisitExpressionList(node.Arguments);
            if (args != node.Arguments)
            {
                return node.Members != null 
                    ? Expression.New(node.Constructor, args, node.Members) 
                    : Expression.New(node.Constructor, args);
            }
            return node;
        }

        /// <summary>
        /// Visits the children of the <see cref="MemberInitExpression" />.
        /// </summary>
        /// <param name="node">
        /// The expression to visit.
        /// </param>
        /// <returns>
        /// The modified expression, if it or any subexpression was modified;
        /// otherwise, returns the original expression.
        /// </returns>
        protected virtual Expression VisitMemberInit(MemberInitExpression node)
        {
            var n = this.VisitNew(node.NewExpression);
            var bindings = this.VisitBindingList(node.Bindings);
            if (n != node.NewExpression || bindings != node.Bindings)
            {
                return Expression.MemberInit(n, bindings);
            }
            return node;
        }

        /// <summary>
        /// Visits the children of the <see cref="ListInitExpression" />.
        /// </summary>
        /// <param name="node">
        /// The expression to visit.
        /// </param>
        /// <returns>
        /// The modified expression, if it or any subexpression was modified;
        /// otherwise, returns the original expression.
        /// </returns>
        protected virtual Expression VisitListInit(ListInitExpression node)
        {
            var n = this.VisitNew(node.NewExpression);
            var initializers = this.VisitElementInitializerList(node.Initializers);
            if (n != node.NewExpression || initializers != node.Initializers)
            {
                return Expression.ListInit(n, initializers);
            }
            return node;
        }

        /// <summary>
        /// Visits the children of the <see cref="NewArrayExpression" />.
        /// </summary>
        /// <param name="node">
        /// The expression to visit.
        /// </param>
        /// <returns>
        /// The modified expression, if it or any subexpression was modified;
        /// otherwise, returns the original expression.
        /// </returns>
        protected virtual Expression VisitNewArray(NewArrayExpression node)
        {
            IEnumerable<Expression> exprs = this.VisitExpressionList(node.Expressions);
            if (exprs != node.Expressions)
            {
                return node.NodeType == ExpressionType.NewArrayInit 
                    ? Expression.NewArrayInit(node.Type.GetElementType(), exprs) 
                    : Expression.NewArrayBounds(node.Type.GetElementType(), exprs);
            }
            return node;
        }

        /// <summary>
        /// Visits the children of the <see cref="InvocationExpression" />.
        /// </summary>
        /// <param name="node">
        /// The expression to visit.
        /// </param>
        /// <returns>
        /// The modified expression, if it or any subexpression was modified;
        /// otherwise, returns the original expression.
        /// </returns>
        protected virtual Expression VisitInvocation(InvocationExpression node)
        {
            IEnumerable<Expression> args = this.VisitExpressionList(node.Arguments);
            var expr = this.Visit(node.Expression);
            if (args != node.Arguments || expr != node.Expression)
            {
                return Expression.Invoke(expr, args);
            }
            return node;
        }
    }
}
#endif