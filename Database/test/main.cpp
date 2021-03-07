#include <iostream>
#include "gtest/gtest.h"
#include "sqlite3/sqlite3.h"
#include "main.h"
#define SQLITE_VERSION_INT 3034001
#define DB_PATH "db.sqlite"

using namespace std;

class QueryTest : public ::testing::Test
{
public:
    void SetUp() override
    {
        Connect(DB_PATH);
    }

    void TearDown() override
    {
        Disconnect();
    }
};

class DataBasedTest : public QueryTest
{
protected:
    void SetUp() override
    {
        QueryTest::SetUp();
        Reset();
    }

    void TearDown() override
    {
        QueryTest::TearDown();
    }
};

TEST(VersionTest, ReturnsCorrectVersion)
{
    ASSERT_EQ(GetSqliteVersion(), SQLITE_VERSION_INT);
}

TEST(ConnectionTest, ConnectsProperly)
{
    ASSERT_EQ(SQLITE_OK, Connect(DB_PATH));
}

TEST(ConnectionTest, DisconnectsProperly)
{
    ASSERT_EQ(SQLITE_OK, Disconnect());
}

TEST_F(QueryTest, ResetsTables)
{
    ASSERT_EQ(SQLITE_OK, Reset());
}

TEST_F(DataBasedTest, FetchesCorrectExtensionId)
{
    ASSERT_EQ(2, GetExtensionId("neat-pack"));
    ASSERT_EQ(0, GetExtensionId("non-existent-extension"));
}

TEST_F(DataBasedTest, InsertsModel)
{
    ASSERT_EQ(SQLITE_OK, AddModel("test", "unity-neat", 3.14));
}

int main()
{
    testing::InitGoogleTest();
    return RUN_ALL_TESTS();
}