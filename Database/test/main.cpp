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
        ResetTables();
    }

    void TearDown() override
    {
        ResetTables();
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

TEST_F(DataBasedTest, ConnectsAndSetsUp)
{
    ASSERT_EQ(SQLITE_CONSTRAINT, ConnectAndSetup(DB_PATH));
}

TEST_F(QueryTest, ResetsTables)
{
    ASSERT_EQ(SQLITE_OK, ResetTables());
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

TEST_F(DataBasedTest, FetchesModel)
{
    ASSERT_EQ(SQLITE_OK, AddModel("test", "unity-neat", 3.14));

    Model *model = GetModel(1);
    ASSERT_STREQ("test", model->content);
    ASSERT_EQ(1, model->extension_id);
    ASSERT_FLOAT_EQ(3.14, model->fitness);

    Model *model2 = GetModel(2);
    ASSERT_EQ(NULL, model2->content);
    ASSERT_EQ(-1, model2->extension_id);
    ASSERT_FLOAT_EQ(-1, model2->fitness);

    DeleteModel(model);
    DeleteModel(model2);
}

TEST_F(DataBasedTest, FetchesBestModel)
{
    AddModel("test", "unity-neat", 3.14);
    AddModel("test2", "neat-pack", 5.7);
    AddModel("test3", "neat-pack", 13.5);
    AddModel("test4", "unity-neat", 0);
    AddModel("test5", "unity-neat", -3);
    AddModel("test6", "unity-neat", 10.3);
    AddModel("test7", "unity-neat", 25.2);
    AddModel("test8", "unity-neat", 1.5);

    ModelCollection *collection = GetBestModels(5);
    ASSERT_FLOAT_EQ(25.2, collection->models[0]->fitness);
    ASSERT_FLOAT_EQ(13.5, collection->models[1]->fitness);
    ASSERT_FLOAT_EQ(10.3, collection->models[2]->fitness);
    ASSERT_FLOAT_EQ(5.7, collection->models[3]->fitness);
    ASSERT_FLOAT_EQ(3.14, collection->models[4]->fitness);

    DeleteCollection(collection);
}

int main()
{
    testing::InitGoogleTest();
    return RUN_ALL_TESTS();
}