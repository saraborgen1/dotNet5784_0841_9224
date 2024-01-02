#pragma once
#include <iostream>
#include <list>
#include <string>
using namespace std;
class Node
{
	string content;
	list<Node*> responses;
	bool isLeaf;//flag if its a leaf
public:
	//con
	Node(string v) { isLeaf = true;  content = v; }
	friend class tree;
	friend class treeList;
};
