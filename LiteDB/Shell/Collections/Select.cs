﻿using System;
using System.Collections.Generic;

namespace LiteDB.Shell
{
    internal class Select : BaseCollection, ICommand
    {
        public bool IsCommand(StringScanner s)
        {
            return this.IsCollectionCommand(s, "select");
        }

        public IEnumerable<BsonValue> Execute(StringScanner s, LiteEngine engine)
        {
            var col = this.ReadCollection(engine, s);
            var expression = BsonExpression.ReadExpression(s, false);
            var output = this.ReadBsonValue(s);

            s.Scan(@"\s*where\s*");

            var query = this.ReadQuery(s);
            var skipLimit = this.ReadSkipLimit(s);
            var includes = this.ReadIncludes(s);
            var docs = engine.Find(col, query, includes, skipLimit.Key, skipLimit.Value);

            var expr = expression == null ? null : new BsonExpression(expression);

            foreach(var doc in docs)
            {
                if (expr != null)
                {
                    foreach(var value in expr.Execute(doc, false))
                    {
                        yield return value;
                    }
                }
                else
                {
                    yield return output;
                }
            }
        }
    }
}