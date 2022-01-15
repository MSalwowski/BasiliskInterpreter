﻿namespace BasiliskLang
{
    public enum NodeType
    {
        ProgramRoot,
        Definition,
        Statement,
        Expression,
        Identifier,
        Assignable,
        BlockStatement,
        IfStatement,
        WhileStatement,
        ReturnStatement,
        AssignStatement,
        Call,
        LogicExpression,
        RelationExpression,
        AdditiveExpression,
        MultiplicativeExpression,
        UnaryExpression,
        SimpleExpression,
        Int,
        Double,
        String
    }
}