/*
File: TextOptionsMem.h
Author: Brian Atwell
Date: November 24, 2019
Descriptions: Preforms memory operations on TextOptions Struct
*/

#pragma once

#include "Windows.h"

#define MAX_TEXTOPTIONS 255

typedef struct {
	HBRUSH color;
	RECT rect;
	int fontSize;
	char *cStr;
	int strLen;
} TEXTOPTIONS_A;

typedef struct {
	HBRUSH color;
	RECT rect;
	int fontSize;
	wchar_t *cStr;
	int strLen;
} TEXTOPTIONS_W;

#ifdef UNICODE
typedef TEXTOPTIONS_W TEXTOPTIONS;
#else
typedef TEXTOPTIONS_A TEXTOPTIONS;
#endif

//TEXTOPTIONS textOptsArray[MAX_TEXTOPTIONS];

bool AddTextOptions(TEXTOPTIONS* textOpts);

bool RemoveTextOptions(TEXTOPTIONS* textOpts);

void FindTextOptionsByString(char* textStr, int len, TEXTOPTIONS* textOpts);

void TextOptionsAtPos(int pos, TEXTOPTIONS* textOpts);

int TextOptionsLength();


