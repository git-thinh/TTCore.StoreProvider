using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using CamlexNET;
using Microsoft.SharePoint.Client;

namespace Mascot.SharePoint.Provider
{
    public class CamlHelper
    {
        /// <summary>
        /// Create select list of fields joined by comma
        /// </summary>
        /// <param name="columns"></param>
        /// <returns></returns>
        public static string Select(params string[] columns)
        {
            return string.Join(",", columns);
        }

        /// <summary>
        /// Get CAML ViewFields part of Query. Support Replace ID column
        /// </summary>
        /// <param name="fieldList">List of Field name separated with comma</param>
        /// <returns></returns>
        public static string ViewFields(string fieldList)
        {
            return RefineCamlexString(Camlex.Query()
                    .ViewFields(fieldList.Split(new[] { ',' }))
                    .ToString());
        }

        /// <summary>
        /// Get filter query
        /// </summary>
        /// <param name="exp">Filter Expression</param>
        /// <param name="isLookup">Set to true in case of using IN tag of CAML. This work if only one field in filter params</param>
        /// <returns></returns>
        public static string Filter(Expression<Func<ListItem, bool>> exp, bool isLookup = false)
        {
            if (!isLookup)
            {
                return RefineCamlexString(Camlex.Query().Where(exp).ToString());
            }

            return RefineCamlexString(Camlex.Query().Where(exp).ToString().Replace("<FieldRef ", "<FieldRef LookupId=\"TRUE\" "));
        }

        /// <summary>
        /// Get filter query
        /// </summary>
        /// <param name="exp">Filter Expression</param>
        /// <param name="lookupField">Which field need to be indicated as lookup field in where clause</param>
        /// <returns></returns>
        public static string Filter(Expression<Func<ListItem, bool>> exp, string lookupField)
        {
            if (string.IsNullOrWhiteSpace(lookupField))
            {
                return RefineCamlexString(Camlex.Query().Where(exp).ToString());
            }

            var where = Camlex.Query().Where(exp).ToString();
            return RefineCamlexString(RefineLookupString(where, lookupField));
        }

        /// <summary>
        /// Get filter query. Support order by clause
        /// </summary>
        /// <param name="exp">Filter expression</param>
        /// <param name="order">Order by expression</param>
        /// <param name="lookupField">Specific lookup field to filter</param>
        /// <returns></returns>
        public static string Filter(Expression<Func<ListItem, bool>> exp, Expression<Func<ListItem, object>> order, string lookupField = "")
        {
            if (string.IsNullOrWhiteSpace(lookupField))
            {
                return RefineCamlexString(Camlex.Query().Where(exp).OrderBy(order).ToString());
            }

            var where = Camlex.Query().Where(exp).OrderBy(order).ToString();
            return RefineCamlexString(RefineLookupString(where, lookupField));
        }

        /// <summary>
        /// Get filter query. Support order by clauses
        /// </summary>
        /// <param name="exp">Filter expression</param>
        /// <param name="order">Order by expressions</param>
        /// <param name="lookupField">Specific lookup field to filter</param>
        /// <returns></returns>
        public static string Filter(Expression<Func<ListItem, bool>> exp, IEnumerable<Expression<Func<ListItem, object>>> order, string lookupField = "")
        {
            if (string.IsNullOrWhiteSpace(lookupField))
            {
                return RefineCamlexString(Camlex.Query().Where(exp).OrderBy(order).ToString());
            }

            var where = Camlex.Query().Where(exp).OrderBy(order).ToString();
            return RefineCamlexString(RefineLookupString(where, lookupField));
        }

        /// <summary>
        /// Sort by a field
        /// </summary>
        /// <param name="orderby"></param>
        /// <returns></returns>
        public static string OrderBy(Expression<Func<ListItem, object>> orderby)
        {
            return RefineCamlexString(Camlex.Query().OrderBy(orderby).ToString());
        }

        /// <summary>
        /// Sort by multiple fields
        /// </summary>
        /// <param name="orderby"></param>
        /// <returns></returns>
        public static string OrderBy(IEnumerable<Expression<Func<ListItem, object>>> orderby)
        {
            return RefineCamlexString(Camlex.Query().OrderBy(orderby).ToString());
        }

        /// <summary>
        /// Get filter query with AND operator from expressions
        /// </summary>
        /// <param name="exps">List of expressions that need to AND conjuntion</param>
        /// <param name="allTrue">Which condition conjuntion to use. Default is AND, otherwise set this to false to get OR</param>
        /// <param name="lookupField">Specific lookup field to filter</param>
        /// <returns></returns>
        public static string Filter(IEnumerable<Expression<Func<ListItem, bool>>> exps, bool allTrue = true, string lookupField = "")
        {
            var where = allTrue ? Camlex.Query().WhereAll(exps).ToString() : Camlex.Query().WhereAny(exps).ToString();

            return RefineCamlexString(string.IsNullOrWhiteSpace(lookupField) ? @where : RefineLookupString(@where, lookupField));
        }

        /// <summary>
        /// Get filter query with AND operator from expressions
        /// </summary>
        /// <param name="exps">List of expressions that need to AND conjuntion</param>
        /// <param name="order">Single order expression</param>
        /// <param name="allTrue">Which condition conjuntion to use. Default is AND, otherwise set this to false to get OR</param>
        /// <param name="lookupField">Specific lookup field to filter</param>
        /// <returns></returns>
        public static string Filter(IEnumerable<Expression<Func<ListItem, bool>>> exps, Expression<Func<ListItem, object>> order, bool allTrue = true, string lookupField = "")
        {
            var where = allTrue ? Camlex.Query().WhereAll(exps).OrderBy(order).ToString() : Camlex.Query().WhereAny(exps).OrderBy(order).ToString();

            return RefineCamlexString(string.IsNullOrWhiteSpace(lookupField) ? @where : RefineLookupString(@where, lookupField));
        }

        /// <summary>
        /// Get filter query with AND operator from expressions
        /// </summary>
        /// <param name="exps">List of expressions that need to AND conjuntion</param>
        /// <param name="order">Multiple order expressions</param>
        /// <param name="allTrue">Which condition conjuntion to use. Default is AND, otherwise set this to false to get OR</param>
        /// <param name="lookupField">Specific lookup field to filter</param>
        /// <returns></returns>
        public static string Filter(IEnumerable<Expression<Func<ListItem, bool>>> exps, IEnumerable<Expression<Func<ListItem, object>>> order, bool allTrue = true, string lookupField = "")
        {
            var where = allTrue ? Camlex.Query().WhereAll(exps).OrderBy(order).ToString() : Camlex.Query().WhereAny(exps).OrderBy(order).ToString();

            return RefineCamlexString(string.IsNullOrWhiteSpace(lookupField) ? @where : RefineLookupString(@where, lookupField));
        }

        /// <summary>
        /// Replace lookup field generated text with missing LookupId=True
        /// </summary>
        /// <param name="input"></param>
        /// <param name="lookupField"></param>
        /// <returns></returns>
        private static string RefineLookupString(string input, string lookupField)
        {
            // in case we have multiple lookup
            if (lookupField.IndexOf(',') > 0)
            {
                var lookups = lookupField.Split(new[] { ',' });
                input = lookups.Aggregate(input, (current, field) => current.Replace(string.Format("<FieldRef Name=\"{0}\"", field), string.Format("<FieldRef Name=\"{0}\" LookupId=\"TRUE\" ", field)));
                return input;
            }

            return input.Replace(string.Format("<FieldRef Name=\"{0}\"", lookupField), string.Format("<FieldRef Name=\"{0}\" LookupId=\"TRUE\" ", lookupField));
        }

        /// <summary>
        /// Replace special field like Id
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        private static string RefineCamlexString(string input)
        {
            return input.Replace("FieldRef Name=\"Id\"", "FieldRef Name=\"ID\"");
        }

        /// <summary>
        /// Get inner joins part of CAML
        /// </summary>
        /// <param name="exps"></param>
        /// <returns></returns>
        public static string InnerJoins(params Expression<Func<ListItem, object>>[] exps)
        {
            var query = Camlex.Query();
            query = exps.Aggregate(query, (current, exp) => current.InnerJoin(exp));
            return RefineCamlexString(query.ToString());
        }

        /// <summary>
        /// Get joins part of CAML
        /// </summary>
        /// <param name="exps"></param>
        /// <returns></returns>
        public static string LeftJoins(params Expression<Func<ListItem, object>>[] exps)
        {
            var query = Camlex.Query();
            query = exps.Aggregate(query, (current, exp) => current.LeftJoin(exp));
            return RefineCamlexString(query.ToString());
        }

        /// <summary>
        /// Get ProjectedFields part of CAML
        /// </summary>
        /// <param name="exps"></param>
        /// <returns></returns>
        public static string ProjectedFields(params Expression<Func<ListItem, object>>[] exps)
        {
            var query = Camlex.Query();
            query = exps.Aggregate(query, (current, exp) => current.ProjectedField(exp));
            return RefineCamlexString(query.ToString());
        }
    }
}