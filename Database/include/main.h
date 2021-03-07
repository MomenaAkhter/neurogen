#ifndef __MAIN_H__
#define __MAIN_H__

extern "C" int GetSqliteVersion();
extern "C" int Connect(const char *path);
extern "C" int Disconnect();
extern "C" int GetExtensionId(const char *);
extern "C" int Reset();
extern "C" int AddModel(const char *, const char *, float);

#endif // __MAIN_H__