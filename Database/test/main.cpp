#include <iostream>
#include "gtest/gtest.h"
#include "sqlite3/sqlite3.h"
#include "main.h"
#define SQLITE_VERSION_INT 3034001
#define DB_PATH "db.sqlite"

using namespace std;

class QueryTest : public ::testing::Test
{
protected:
    void SetUp() override
    {
        connect(DB_PATH);
    }

    void TearDown() override
    {
        disconnect();
    }
};

TEST(VersionTest, ReturnsCorrectVersion)
{
    ASSERT_EQ(getSqliteVersion(), SQLITE_VERSION_INT);
}

TEST(ConnectionTest, ConnectsProperly)
{
    ASSERT_EQ(SQLITE_OK, connect(DB_PATH));
}

TEST(ConnectionTest, DisconnectsProperly)
{
    ASSERT_EQ(SQLITE_OK, disconnect());
}

TEST_F(QueryTest, FetchesCorrectExtensionId)
{
    ASSERT_EQ(2, getExtensionId("neat-pack"));
    ASSERT_EQ(0, getExtensionId("non-existent-extension"));
}

int main()
{
    testing::InitGoogleTest();
    return RUN_ALL_TESTS();
}