#if NET35
using System.Collections.Generic;
using System.Collections.ObjectModel;


namespace System.Linq.Expressions
{
    public abstract class ExpressionVisitor
    {
        protected ExpressionVisitor() { }

        public virtual Expression Visit(Expression exp)
        {
            if (exp == null)
            {
                return exp;
            }
            switch (exp.NodeType)
            {
                case ExpressionType.Negate:
                case ExpressionType.NegateChecked:
                case ExpressionType.Not:
                case ExpressionType.Convert:
                case ExpressionType.ConvertChecked:
                case ExpressionType.ArrayLength:
                case ExpressionType.Quote:
                case ExpressionType.TypeAs:
                    return this.VisitUnary((UnaryExpression)exp);
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
                    return this.VisitBinary((BinaryExpression)exp);
                case ExpressionType.TypeIs:
                    return this.VisitTypeIs((TypeBinaryExpression)exp);
                case ExpressionType.Conditional:
                    return this.VisitConditional((ConditionalExpression)exp);
                case ExpressionType.Constant:
                    return this.VisitConstant((ConstantExpression)exp);
                case ExpressionType.Parameter:
                    return this.VisitParameter((ParameterExpression)exp);
                case ExpressionType.MemberAccess:
                    return this.VisitMemberAccess((MemberExpression)exp);
                case ExpressionType.Call:
                    return this.VisitMethodCall((MethodCallExpression)exp);
                case ExpressionType.Lambda:
                    return this.VisitLambda((LambdaExpression)exp);
                case ExpressionType.New:
                    return this.VisitNew((NewExpression)exp);
                case ExpressionType.NewArrayInit:
                case ExpressionType.NewArrayBounds:
                    return this.VisitNewArray((NewArrayExpression)exp);
                case ExpressionType.Invoke:
                    return this.VisitInvocation((InvocationExpression)exp);
                case ExpressionType.MemberInit:
                    return this.VisitMemberInit((MemberInitExpression)exp);
                case ExpressionType.ListInit:
                    return this.VisitListInit((ListInitExpression)exp);
                default:
                    throw new InvalidOperationException(string.Format("Unhandled expression type: '{0}'", exp.NodeType));
            }
        }

        protected virtual MemberBinding VisitBinding(MemberBinding binding)
        {
            switch (binding.BindingType)
            {
                case MemberBindingType.Assignment:
                    return this.VisitMemberAssignment((MemberAssignment)binding);
                case MemberBindingType.MemberBinding:
                    return this.VisitMemberMemberBinding((MemberMemberBinding)binding);
                case MemberBindingType.ListBinding:
                    return this.VisitMemberListBinding((MemberListBinding)binding);
                default:
                    throw new InvalidOperationException(string.Format("Unhandled binding type '{0}'", binding.BindingType));
            }
        }

        protected virtual ElementInit VisitElementInitializer(ElementInit initializer)
        {
            var arguments = this.VisitExpressionList(initializer.Arguments);
            return arguments != initializer.Arguments ? Expression.ElementInit(initializer.AddMethod, arguments) : initializer;
        }

        protected virtual Expression VisitUnary(UnaryExpression u)
        {
            var operand = this.Visit(u.Operand);
            return operand != u.Operand ? Expression.MakeUnary(u.NodeType, operand, u.Type, u.Method) : u;
        }

        protected virtual Expression VisitBinary(BinaryExpression b)
        {
            var left = this.Visit(b.Left);
            var right = this.Visit(b.Right);
            var conversion = this.Visit(b.Conversion);
            if (left != b.Left || right != b.Right || conversion != b.Conversion)
            {
                if (b.NodeType == ExpressionType.Coalesce && b.Conversion != null)
                {
                    return Expression.Coalesce(left, right, conversion as LambdaExpression);
                }
                return Expression.MakeBinary(b.NodeType, left, right, b.IsLiftedToNull, b.Method);
            }
            return b;
        }

        protected virtual Expression VisitTypeIs(TypeBinaryExpression b)
        {
            var expr = this.Visit(b.Expression);
            return expr != b.Expression ? Expression.TypeIs(expr, b.TypeOperand) : b;
        }

        protected virtual Expression VisitConstant(ConstantExpression c)
        {
            return c;
        }

        protected virtual Expression VisitConditional(ConditionalExpression c)
        {
            var test = this.Visit(c.Test);
            var ifTrue = this.Visit(c.IfTrue);
            var ifFalse = this.Visit(c.IfFalse);
            if (test != c.Test || ifTrue != c.IfTrue || ifFalse != c.IfFalse)
            {
                return Expression.Condition(test, ifTrue, ifFalse);
            }
            return c;
        }

        protected virtual Expression VisitParameter(ParameterExpression p)
        {
            return p;
        }

        protected virtual Expression VisitMemberAccess(MemberExpression m)
        {
            var exp = this.Visit(m.Expression);
            return exp != m.Expression ? Expression.MakeMemberAccess(exp, m.Member) : m;
        }

        protected virtual Expression VisitMethodCall(MethodCallExpression m)
        {
            var obj = this.Visit(m.Object);
            IEnumerable<Expression> args = this.VisitExpressionList(m.Arguments);
            if (obj != m.Object || args != m.Arguments)
            {
                return Expression.Call(obj, m.Method, args);
            }
            return m;
        }

        protected virtual ReadOnlyCollection<Expression> VisitExpressionList(ReadOnlyCollection<Expression> original)
        {
            List<Expression> list = null;
            for (int i = 0, n = original.Count; i < n; i++)
            {
                var p = this.Visit(original[i]);
                if (list != null)
                {
                    list.Add(p);
                }
                else if (p != original[i])
                {
                    list = new List<Expression>(n);
                    for (var j = 0; j < i; j++)
                    {
                        list.Add(original[j]);
                    }
                    list.Add(p);
                }
            }
            return list != null ? list.AsReadOnly() : original;
        }

        protected virtual MemberAssignment VisitMemberAssignment(MemberAssignment assignment)
        {
            var e = this.Visit(assignment.Expression);
            return e != assignment.Expression ? Expression.Bind(assignment.Member, e) : assignment;
        }

        protected virtual MemberMemberBinding VisitMemberMemberBinding(MemberMemberBinding binding)
        {
            var bindings = this.VisitBindingList(binding.Bindings);
            return bindings != binding.Bindings ? Expression.MemberBind(binding.Member, bindings) : binding;
        }

        protected virtual MemberListBinding VisitMemberListBinding(MemberListBinding binding)
        {
            var initializers = this.VisitElementInitializerList(binding.Initializers);
            return initializers != binding.Initializers ? Expression.ListBind(binding.Member, initializers) : binding;
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

        protected virtual Expression VisitLambda(LambdaExpression lambda)
        {
            var body = this.Visit(lambda.Body);
            return body != lambda.Body 
                ? Expression.Lambda(lambda.Type, body, lambda.Parameters) 
                : lambda;
        }

        protected virtual NewExpression VisitNew(NewExpression nex)
        {
            IEnumerable<Expression> args = this.VisitExpressionList(nex.Arguments);
            if (args != nex.Arguments)
            {
                return nex.Members != null 
                    ? Expression.New(nex.Constructor, args, nex.Members) 
                    : Expression.New(nex.Constructor, args);
            }
            return nex;
        }

        protected virtual Expression VisitMemberInit(MemberInitExpression init)
        {
            var n = this.VisitNew(init.NewExpression);
            var bindings = this.VisitBindingList(init.Bindings);
            if (n != init.NewExpression || bindings != init.Bindings)
            {
                return Expression.MemberInit(n, bindings);
            }
            return init;
        }

        protected virtual Expression VisitListInit(ListInitExpression init)
        {
            var n = this.VisitNew(init.NewExpression);
            var initializers = this.VisitElementInitializerList(init.Initializers);
            if (n != init.NewExpression || initializers != init.Initializers)
            {
                return Expression.ListInit(n, initializers);
            }
            return init;
        }

        protected virtual Expression VisitNewArray(NewArrayExpression na)
        {
            IEnumerable<Expression> exprs = this.VisitExpressionList(na.Expressions);
            if (exprs != na.Expressions)
            {
                return na.NodeType == ExpressionType.NewArrayInit 
                    ? Expression.NewArrayInit(na.Type.GetElementType(), exprs) 
                    : Expression.NewArrayBounds(na.Type.GetElementType(), exprs);
            }
            return na;
        }

        protected virtual Expression VisitInvocation(InvocationExpression iv)
        {
            IEnumerable<Expression> args = this.VisitExpressionList(iv.Arguments);
            var expr = this.Visit(iv.Expression);
            if (args != iv.Arguments || expr != iv.Expression)
            {
                return Expression.Invoke(expr, args);
            }
            return iv;
        }
    }
}
#endif