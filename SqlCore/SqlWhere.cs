
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using SqlCore;
using TCore.Sql.Core;
using static System.String;

namespace TCore.Sql.SqlClient;

// ===============================================================================
//  S Q L  W H E R E
// ===============================================================================
public class SqlWhere
{
    public TableAliases Aliases { get; } = new();
    private List<Clause> m_clauses = new();

    public enum Op
    {
        And,
        AndNot,
        Or,
        OrNot
    }

    public struct Clause
    {
        public Op op;
        public string clause;
        public SqlSelect? subclause;
    }


    /* A D D  A L I A S */
    /*----------------------------------------------------------------------------
        %%Function: AddAlias
        %%Qualified: BhSvc.SqlWhere.AddAlias
        %%Contact: rlittle

    ----------------------------------------------------------------------------*/
    public string AddAlias(string sTable)
    {
        return Aliases.AddAlias(sTable);
    }

    /* A D D  A L I A S E S */
    /*----------------------------------------------------------------------------
        %%Function: AddAliases
        %%Qualified: BhSvc.SqlWhere.AddAliases
        %%Contact: rlittle

        add a collection of aliases			
    ----------------------------------------------------------------------------*/
    public void AddAliases(Dictionary<string, string> mpAliases)
    {
        Aliases.SetAliases(mpAliases);
    }

    public void AddAliases(TableAliases aliases)
    {
        Aliases.SetAliases(aliases);
    }

    /* A D D  A L I A S */
    /*----------------------------------------------------------------------------
        %%Function: AddAlias
        %%Qualified: BhSvc.SqlWhere.AddAlias
        %%Contact: rlittle

    ----------------------------------------------------------------------------*/
    public string AddAlias(string sTable, string sAlias)
    {
        Aliases.AddAlias(sTable, sAlias);

        return sAlias;
    }


    /* C L E A R */
    /*----------------------------------------------------------------------------
        %%Function: Clear
        %%Qualified: BhSvc.SqlWhere.Clear
        %%Contact: rlittle

    ----------------------------------------------------------------------------*/
    public void Clear()
    {
        m_clauses = new List<Clause>();
    }

    /* B H  S Q L  W H E R E */
    /*----------------------------------------------------------------------------
        %%Function: SqlWhere
        %%Qualified: BhSvc.SqlWhere.SqlWhere
        %%Contact: rlittle

    ----------------------------------------------------------------------------*/
    public SqlWhere()
    {
        Clear();
    }

    /* S T A R T  G R O U P */
    /*----------------------------------------------------------------------------
        %%Function: StartGroup
        %%Qualified: BhSvc.SqlWhere.StartGroup
        %%Contact: rlittle

    ----------------------------------------------------------------------------*/
    public void StartGroup(Op op)
    {
        Clause c;

        c.op = op;
        c.clause = "(";
        c.subclause = null;

        m_clauses.Add(c);
    }

    /* E N D  G R O U P */
    /*----------------------------------------------------------------------------
        %%Function: EndGroup
        %%Qualified: BhSvc.SqlWhere.EndGroup
        %%Contact: rlittle

    ----------------------------------------------------------------------------*/
    public void EndGroup()
    {
        Clause c;

        c.op = Op.And;
        c.clause = ")";
        c.subclause = null;

        m_clauses.Add(c);
    }

    /* A D D */
    /*----------------------------------------------------------------------------
        %%Function: Add
        %%Qualified: BhSvc.SqlWhere.Add
        %%Contact: rlittle

    ----------------------------------------------------------------------------*/
    public void Add(string s, Op op)
    {
        Clause c;

        c.op = op;
        c.clause = s;
        c.subclause = null;

        m_clauses.Add(c);
    }

    public void AddEx(string s, Op op)
    {
        s = Aliases.ExpandAliases(s);

        Add(s, op);
    }

    public void AddSubclause(string sFormat, SqlSelect sqls, Op op)
    {
        Clause c;

        c.op = op;
        c.clause = sFormat;
        c.subclause = sqls;

        m_clauses.Add(c);
    }

    /* G E T  W H E R E */
    /*----------------------------------------------------------------------------
        %%Function: GetWhere
        %%Qualified: BhSvc.SqlWhere.GetWhere
        %%Contact: rlittle
     
        get the entire clause, combining the give base and any already set
        group by
    ----------------------------------------------------------------------------*/
    public string GetWhere(string? sBase)
    {
        if (sBase == null)
            sBase = "";

        StringBuilder sb = new StringBuilder(256);

        sb.Append(Aliases.ExpandAliases(sBase));

        if (m_clauses.Count == 0)
            return sb.ToString();

        sb.Append(" WHERE ");
        sb.Append(GetClause());

        return sb.ToString();
    }


    /* G E T  C L A U S E */
    /*----------------------------------------------------------------------------
        %%Function: GetClause
        %%Qualified: tw.twsvc:SqlWhere.GetClause
        %%Contact: rlittle

        Just get the where portion....don't include WHERE
    ----------------------------------------------------------------------------*/
    public string GetClause()
    {
        if (m_clauses.Count == 0)
            return "";
        //			else if (m_clauses.Count == 1)
        //				return ExpandAliases(String.Format("{0}", m_clauses[0].clause));
        else
        {
            bool fSkipOp = true;
            bool fSkipGroupClose = false;

            string sWhere = "";

            for (int i = 0; i < m_clauses.Count; i++)
            {
                Clause c = m_clauses[i];
                string sClause = c.clause;
                string sOp;
                string sUnary = "";

                if (c.subclause != null)
                {
                    sClause = Format(c.clause, Format("({0})", c.subclause.ToString()));
                }

                if (c.op == Op.And)
                {
                    sOp = "AND";
                }
                else if (c.op == Op.AndNot)
                {
                    sOp = "AND";
                    sUnary = " NOT ";
                }
                else if (c.op == Op.Or)
                {
                    sOp = "OR";
                }
                else if (c.op == Op.OrNot)
                {
                    sOp = "OR";
                    sUnary = " NOT ";
                }
                else
                {
                    sOp = "";
                }

                if (sClause == "(")
                {
                    // simple skip of ()...
                    if (i + 1 >= m_clauses.Count || m_clauses[i + 1].clause != ")")
                    {
                        if (fSkipOp)
                        {
                            sWhere += sUnary + "(";
                            fSkipOp = false;
                        }
                        else
                            sWhere += Format(" {0}{1} (", sOp, sUnary);

                        ;

                        fSkipOp = true;
                    }
                    else
                        fSkipGroupClose = true;
                }
                else if (sClause == ")")
                {
                    if (!fSkipGroupClose)
                    {
                        sWhere += ") ";
                        fSkipOp = false;
                    }

                    fSkipGroupClose = false;
                }
                else
                {
                    if (m_clauses.Count == 1)
                        fSkipOp = true;

                    sWhere += Format(" {0}{1} {2}", fSkipOp ? "" : sOp, sUnary, sClause);
                    fSkipOp = false;
                }
            }

            return Aliases.ExpandAliases(sWhere);
        }
    }


    [TestFixture]
    public class SqlUnitTest
    {
        [Test]
        public void AliasUnitTest()
        {
            SqlWhere sw;
            string sExpected;
            sw = new SqlWhere();

            string sUserAlias = sw.AddAlias("tw_user");
            sw.AddAlias("tw_locks", "TWL");

            sw.Add("$$tw_user$$.LockID = $$tw_locks$$.LockID", Op.And);
            sExpected = "base WHERE   S0.LockID = TWL.LockID";
            Assert.AreEqual(sExpected, sw.GetWhere("base"));
        }

        [Test]
        public void GroupUnitTest_UnaryOperator()
        {
            string sExpected;
            SqlWhere sw;

            sw = new SqlWhere();
            sw.Add("A=1", Op.And);
            sw.StartGroup(Op.AndNot);
            sw.StartGroup(Op.And);
            sw.EndGroup();
            sw.Add("B=1", Op.Or);
            sw.EndGroup();
            sw.Add("C=1", Op.And);

            sExpected = "base WHERE   A=1 AND NOT  (  B=1)  AND C=1";
            Assert.AreEqual(sExpected, sw.GetWhere("base"));
        }

        [Test]
        public void SelectUnitTest()
        {
            String sExpected = "SELECT foo FROM bar WHERE   baz=boo";
            SqlSelect sqls = new SqlSelect("SELECT foo FROM bar");

            sqls.Where.Add("baz=boo", Op.And);
            Assert.AreEqual(sExpected, sqls.ToString());
        }

        [Test]
        public void SelectUnitTest_OrderBy()
        {
            String sExpected = "SELECT foo FROM bar WHERE   baz=boo ORDER BY foo DESC";
            SqlSelect sqls = new SqlSelect("SELECT foo FROM bar");

            sqls.AddOrderBy("foo DESC");
            sqls.Where.Add("baz=boo", Op.And);
            Assert.AreEqual(sExpected, sqls.ToString());
        }

        [Test]
        public void SelectUnitTest_Aliases()
        {
            String sExpected = "SELECT AI.foo FROM alias1 AI WHERE   AI.baz=boo";
            Dictionary<string, string> mpAliases = new Dictionary<string, string>
                                                   {
                                                       {"alias1", "AI"},
                                                       {"alias2", "AII"},
                                                   };
            SqlSelect sqls = new SqlSelect("SELECT $$alias1$$.foo FROM $$#alias1$$", mpAliases);

            sqls.Where.Add("$$alias1$$.baz=boo", Op.And);
            Assert.AreEqual(sExpected, sqls.ToString());
        }

        [Test]
        public void SelectUnitTest_OrderByAliases()
        {
            String sExpected = "SELECT AI.foo FROM alias1 AI WHERE   AI.baz=boo ORDER BY AI.foo DESC";
            Dictionary<string, string> mpAliases = new Dictionary<string, string>
                                                   {
                                                       {"alias1", "AI"},
                                                       {"alias2", "AII"},
                                                   };
            SqlSelect sqls = new SqlSelect("SELECT $$alias1$$.foo FROM $$#alias1$$", mpAliases);

            sqls.AddOrderBy("$$alias1$$.foo DESC");
            sqls.Where.Add("$$alias1$$.baz=boo", Op.And);
            Assert.AreEqual(sExpected, sqls.ToString());
        }

        [Test]
        public void SubclauseUnitTest()
        {
            string sExpected = "base WHERE   A=(SELECT foo FROM bar WHERE   baz=boo)";
            SqlSelect sqlsInner = new SqlSelect("SELECT foo FROM bar");
            SqlWhere sqlwOuter = new SqlWhere();

            sqlsInner.Where.Add("baz=boo", Op.And);
            sqlwOuter.AddSubclause("A={0}", sqlsInner, Op.And);

            Assert.AreEqual(sExpected, sqlwOuter.GetWhere("base"));
        }

        [TestCase(true, true,
            "SELECT FOO.FooValue, BAR.BarValue FROM tbl_foo FOO INNER JOIN tbl_bar BAR ON   FOO.FooKey = BAR.BarKey WHERE   FOO.Match1 = 'Match1' OR (  FOO.Match2_1 = 'M1' AND FOO.Match2_2 = 'M2') ")]
        [TestCase(true, false,
            "SELECT FOO.FooValue, BAR.BarValue FROM tbl_foo FOO INNER JOIN tbl_bar BAR ON   FOO.FooKey = BAR.BarKey WHERE   FOO.Match1 = 'Match1'")]
        [TestCase(false, true,
            "SELECT FOO.FooValue, BAR.BarValue FROM tbl_foo FOO INNER JOIN tbl_bar BAR ON   FOO.FooKey = BAR.BarKey WHERE (  FOO.Match2_1 = 'M1' AND FOO.Match2_2 = 'M2') ")]
        [TestCase(false, false,
            "SELECT FOO.FooValue, BAR.BarValue FROM tbl_foo FOO INNER JOIN tbl_bar BAR ON   FOO.FooKey = BAR.BarKey")]
        [Test]
        public void TestOptionalWhereBoth(bool fFirstClause, bool fSecondClause, string sExpected)
        {
            Dictionary<string, string> mpAliases = new Dictionary<string, string>
                                                   {
                                                       {"tbl_foo", "FOO"},
                                                       {"tbl_bar", "BAR"}
                                                   };


            string sBaseQuery = "SELECT $$tbl_foo$$.FooValue, $$tbl_bar$$.BarValue FROM $$#tbl_foo$$";

            SqlSelect sqls = new SqlSelect(sBaseQuery, mpAliases);
            Assert.AreEqual("SELECT FOO.FooValue, BAR.BarValue FROM tbl_foo FOO", sqls.ToString());

            if (fFirstClause)
                sqls.Where.Add("$$tbl_foo$$.Match1 = 'Match1'", Op.And);

            if (fSecondClause)
            {
                sqls.Where.StartGroup(Op.Or);
                sqls.Where.Add("$$tbl_foo$$.Match2_1 = 'M1'", Op.And);
                sqls.Where.Add("$$tbl_foo$$.Match2_2 = 'M2'", Op.And);
                sqls.Where.EndGroup();
            }

            SqlWhere swInnerJoin = new SqlWhere();
            swInnerJoin.AddAliases(mpAliases);
            swInnerJoin.Add("$$tbl_foo$$.FooKey = $$tbl_bar$$.BarKey", Op.And);
            sqls.AddInnerJoin(new SqlInnerJoin("$$#tbl_bar$$", swInnerJoin));

            Assert.AreEqual(sExpected, sqls.ToString());
        }

        [Test]
        public void SubclauseUnitTest_Aliases()
        {
            string sExpected =
                "SELECT A1O.foo FROM alias1 A1O WHERE   A1O.foo=(SELECT A1I.foo FROM alias1 A1I WHERE   A1I.baz=boo)";
            Dictionary<string, string> mpAliasesInner = new Dictionary<string, string>
                                                        {
                                                            {"alias1", "A1I"},
                                                        };
            Dictionary<string, string> mpAliasesOuter = new Dictionary<string, string>
                                                        {
                                                            {"alias1", "A1O"},
                                                        };

            SqlSelect sqlsInner = new SqlSelect("SELECT $$alias1$$.foo FROM $$#alias1$$", mpAliasesInner);
            SqlSelect sqlsOuter = new SqlSelect("SELECT $$alias1$$.foo FROM $$#alias1$$", mpAliasesOuter);

            sqlsInner.Where.Add("$$alias1$$.baz=boo", Op.And);
            sqlsOuter.Where.AddSubclause("$$alias1$$.foo={0}", sqlsInner, Op.And);

            Assert.AreEqual(sExpected, sqlsOuter.ToString());
        }

        [Test]
        public void GroupUnitTest()
        {
            string sExpected;
            SqlWhere sw;

            sw = new SqlWhere();
            sw.Add("A=1", Op.And);
            sw.StartGroup(Op.And);
            sw.StartGroup(Op.And);
            sw.EndGroup();
            sw.Add("B=1", Op.Or);
            sw.EndGroup();
            sw.Add("C=1", Op.And);

            sExpected = "base WHERE   A=1 AND (  B=1)  AND C=1";
            Assert.AreEqual(sExpected, sw.GetWhere("base"));
        }

        [Test]
        public void GroupUnitTest_GroupAtEnd()
        {
            string sExpected;
            SqlWhere sw;

            sw = new SqlWhere();
            sw.Add("A=1", Op.And);
            sw.StartGroup(Op.And);
            sw.StartGroup(Op.And);
            sw.EndGroup();
            sw.Add("B=1", Op.Or);
            sw.EndGroup();

            sExpected = "base WHERE   A=1 AND (  B=1) ";
            Assert.AreEqual(sExpected, sw.GetWhere("base"));
        }

        [Test]
        public void GroupUnitTests_NestedGroups()
        {
            string sExpected;
            SqlWhere sw;

            sw = new SqlWhere();

            sw.Add("A=1", Op.And);
            sw.Add("B=2", Op.And);
            sw.StartGroup(Op.And);
            sw.StartGroup(Op.And);
            sw.Add("B1=2", Op.And);
            sw.Add("B2=2", Op.And);
            sw.EndGroup();
            sw.StartGroup(Op.Or);
            sw.Add("C1=2", Op.And);
            sw.Add("C2=2", Op.And);
            sw.EndGroup();
            sw.StartGroup(Op.OrNot);
            sw.Add("C1=2", Op.And);
            sw.Add("C2=2", Op.And);
            sw.EndGroup();
            sw.EndGroup();
            sw.Add("C=3", Op.And);

            sExpected =
                "base WHERE   A=1 AND B=2 AND ((  B1=2 AND B2=2)  OR (  C1=2 AND C2=2)  OR NOT  (  C1=2 AND C2=2) )  AND C=3";
            Assert.AreEqual(sExpected, sw.GetWhere("base"));
        }

        [Test]
        public void BasicOperatorUnitTest()
        {
            string sExpected;
            SqlWhere sw = new SqlWhere();

            sw.Add("A=1", Op.And);
            sw.Add("B=2", Op.Or);
            sw.Add("C=3", Op.And);

            sExpected = "base WHERE   A=1 OR B=2 AND C=3";
            Assert.AreEqual(sExpected, sw.GetWhere("base"));
        }

        [Test]
        public void GroupByUnitTest()
        {
            SqlSelect sqls = new SqlSelect("SELECT COUNT(Foo) As CountFoo FROM Foo");

            sqls.AddGroupBy("CountFoo");
            Assert.AreEqual("SELECT COUNT(Foo) As CountFoo FROM Foo GROUP BY CountFoo", sqls.ToString());
        }

        [Test]
        public void GroupByUnitTestWithWhereClause()
        {
            SqlSelect sqls = new SqlSelect("SELECT COUNT(Foo) As CountFoo FROM Foo");

            sqls.Where.Add("Foo = 'Bar'", Op.And);

            sqls.AddGroupBy("CountFoo");
            Assert.AreEqual("SELECT COUNT(Foo) As CountFoo FROM Foo WHERE   Foo = 'Bar' GROUP BY CountFoo", sqls.ToString());
        }

        [Test]
        /* I N N E R  J O I N  U N I T  T E S T */
        /*----------------------------------------------------------------------------
            %%Function: InnerJoinUnitTest
            %%Qualified: BhSvc.SqlWhere.InnerJoinUnitTest
            %%Contact: rlittle

            Test the inner join functionality...
     
            Table structure:
            Order Table:  <idOrder>, <idCustomer>, <idProduct>
            Cust Table: <idCustomer>, <sCustName>, <idCity>
            Prod Table: <idProduct>, <sProdName>
            City Table: <idCity>, <sCityName>, <sMayor>

        ----------------------------------------------------------------------------*/
        public void InnerJoinUnitTest()
        {
            SqlSelect sqls;
            SqlWhere sw;
            string sExpected;
            string sBase;
            string sTest;

            // first query test -- just a readable form:
            // idOrder, sCustName, sCityName, sProdName

            // select Order.idOrder, Cust.sCustName, City.sCityName, Prod.sProdName

            Dictionary<string, string> mpAliases = new Dictionary<string, string>
                                                   {
                                                       {"tblOrder", "TBLO"},
                                                       {"tblCust", "TBLC"},
                                                       {"tblCity", "TBLCY"},
                                                       {"tblProd", "TBLP"}
                                                   };

            sBase =
                "select $$tblOrder$$.idOrder, $$tblCust$$.sCustName, $$tblCity$$.sCityName, $$tblProd$$.sProdName from $$#tblOrder$$";

            sqls = new SqlSelect(sBase, mpAliases);

            sExpected = "select TBLO.idOrder, TBLC.sCustName, TBLCY.sCityName, TBLP.sProdName from tblOrder TBLO " +
                "INNER JOIN tblCust TBLC ON   TBLC.idCust=TBLO.idCust " +
                "INNER JOIN tblCity TBLCY ON   TBLCY.idCity=TBLC.idCity " +
                "INNER JOIN tblProd TBLP ON   TBLP.idProd=TBLO.idProd";

            sw = sqls.Where;

            SqlWhere swIJ;
            swIJ = new SqlWhere();
            swIJ.Add("$$tblCust$$.idCust=$$tblOrder$$.idCust", Op.And);
            sqls.AddInnerJoin(new SqlInnerJoin("$$#tblCust$$", swIJ));

            swIJ = new SqlWhere();
            swIJ.Add("$$tblCity$$.idCity=$$tblCust$$.idCity", Op.And);
            sqls.AddInnerJoin(new SqlInnerJoin("$$#tblCity$$", swIJ));

            swIJ = new SqlWhere();
            swIJ.Add("$$tblProd$$.idProd=$$tblOrder$$.idProd", Op.And);
            sqls.AddInnerJoin(new SqlInnerJoin("$$#tblProd$$", swIJ));

            sTest = sqls.ToString();
            Assert.AreEqual(sExpected, sTest);

            // second test, matching all the orders that relate to Mayor "dudley"...
            //
            // idOrder, scustName, sCityName, sProdName
            // WHERE tblcy.sMayer == 'dudley'

            // since this is the same inner join/select as before, with just some
            // WHERE criteria, we'll build on top of bhsw

            // same sBase as before
            sExpected = "select TBLO.idOrder, TBLC.sCustName, TBLCY.sCityName, TBLP.sProdName from tblOrder TBLO " +
                "INNER JOIN tblCust TBLC ON   TBLC.idCust=TBLO.idCust " +
                "INNER JOIN tblCity TBLCY ON   TBLCY.idCity=TBLC.idCity " +
                "INNER JOIN tblProd TBLP ON   TBLP.idProd=TBLO.idProd " +
                "WHERE   TBLCY.sMayor=='dudley'";

            sw.Add("$$tblCity$$.sMayor=='dudley'", Op.And);

            sTest = sqls.ToString();
            Assert.AreEqual(sExpected, sTest);
        }

        [Test]
        [TestCase("12/25/2013", "'2013-12-25T00:00:00.000'")]
        [TestCase("12/25/2013 23:13:02", "'2013-12-25T23:13:02.000'")]
        [TestCase("12/25/2013 08:00:00Z", "'2013-12-25T00:00:00.000'")]
        [TestCase("12/25/2013 23:13:02.12345", "'2013-12-25T23:13:02.123'")]
        [TestCase(null, "null")]
        public void NullableTests(string sDateTime, string sExpected)
        {
            DateTime? dttm = sDateTime == null ? (DateTime?) null : (DateTime?) DateTime.Parse(sDateTime);
            Assert.AreEqual(sExpected, SqlText.Nullable(dttm));
        }

        [Test]
        [TestCase("foo", "foo")]
        [TestCase("fo'o", "fo''o")]
        public void SqlifyTests(string sInput, string sExpected)
        {
            Assert.AreEqual(sExpected, SqlText.Sqlify(sInput));
        }


    }
}