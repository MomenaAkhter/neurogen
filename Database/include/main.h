#ifndef __MAIN_H__
#define __MAIN_H__

extern "C" int getSqliteVersion();
extern "C" int connect(const char *path);
extern "C" int disconnect();
extern "C" int getExtensionId(const char *);

#endif // __MAIN_H__