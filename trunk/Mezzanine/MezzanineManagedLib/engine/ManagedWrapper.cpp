// ManagedWrapper.cpp

// This code verifies that DllMain is not automatically called
// by the Loader when linked with /noentry. It also checks some
// functions that the CRT initializes.

#include <windows.h>
#include <stdio.h>
#include <string.h>
#include <stdlib.h>
#include <math.h>
#include "_vcclrit.h"

#using <mscorlib.dll>
using namespace System;

public __gc class ManagedWrapper {
public:
	static BOOL minitialize() {
		BOOL retval = TRUE;
		try {
           retval =  __crt_dll_initialize();
		} catch(System::Exception* e) {
			Console::WriteLine(e->Message);
			retval = FALSE;
		}
		return retval;
	}
	static BOOL mterminate() {
		BOOL retval = TRUE;
		try {
            retval = __crt_dll_terminate();
		} catch(System::Exception* e) {
						Console::WriteLine(e->Message);
			retval = FALSE;
		}
		return retval;
	}
};

BOOL WINAPI DllMain(HINSTANCE hModule, DWORD dwReason, LPVOID
lpvReserved) {
	Console::WriteLine(S"DllMain is called...");
	return TRUE;
} /* DllMain */
