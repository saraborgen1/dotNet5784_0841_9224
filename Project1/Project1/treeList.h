#pragma once
#include <iostream>
#include <list>
#include <string>
#include "Node.h"
#include "tree.h"

class treeList
{
	list<tree*> trees;

public:
	treeList() { list<tree*> trees; }
	~treeList();
	void addNewTree(string s);
	void deleteTree(tree*);
	void searchAndPrint(string val);
	bool addResponse(string rt, string prnt, string res);
	bool delResponse(string rt, string res);
	void printTree(string rt);
	void printAllTrees();
	void printSubTree(string rt, string s);
};
