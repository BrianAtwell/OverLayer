#pragma once

typedef struct
{
	void* data;
	int dataSize;
	CLINK_NODE_T *next;
} CLINK_NODE_T;

typedef struct
{
	int listLen;
	CLINK_NODE_T *node;
} CLINK_ROOT_T;

int CLinkPush(CLINK_ROOT_T* root, CLINK_NODE_T* node);

int CLinkRemove(CLINK_ROOT_T* root, CLINK_NODE_T* node, int pos);

int CLinkInsert(CLINK_ROOT_T* root, CLINK_NODE_T* node, int pos);

int CLinkPop(CLINK_ROOT_T* root, CLINK_NODE_T* node);
