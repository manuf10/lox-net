namespace LoxNet.Interpreter.Components
{
    public abstract class Expression
    {
        public class BinaryExpression : Expression
        {
            BinaryExpression(Expression left, Token expressionOperator, Expression right)
            {
                this._left = left;
                this._expressionOperator = expressionOperator;
                this._right = right;
            }

            readonly Expression _left;
            readonly Token _expressionOperator;
            readonly Expression _right;
        }
    }
}