#pragma once
#include "treeList.h"
#include <iostream>
#include <list>
#include <string>
#include "Node.h"
#include "tree.h"

treeList::~treeList()
{
	if (trees.empty())
		return;
	for (list<tree*>::iterator it = trees.begin(); it != trees.end(); it++)
	{
		(**it).~tree();
		//(**it).delResponse((**it).root->content);
		//*it = nullptr;		
	}
	trees.clear();
}

void treeList::addNewTree(string s)
{
	tree* temp = new tree();
	(*temp).addNewTree(s);
	trees.push_back(temp);
}
void treeList::deleteTree(tree* a)
{
	a->~tree();
	trees.remove(a);
}
void treeList::searchAndPrint(string val)
{
	for (list<tree*>::iterator it = trees.begin(); it != trees.end(); it++)
	{
		Node* temp = (**it).search((**it).root, val, (**it).root);
		if (!temp)
		{
			(**it).printTree(temp, 0);
			(**it).printPart(val);
			return;
		}
	}
}
bool treeList::addResponse(string rt, string prnt, string res)
{
	for (list<tree*>::iterator it = trees.begin(); it != trees.end(); it++)
	{
		if ((**it).root->content == rt)
		{
			if (!(**it).search((**it).root, prnt, (**it).root))
				return false;
			(**it).addResponse(prnt, res);
			return true;
		}
			
	}
	return false;
}


bool treeList::delResponse(string rt, string res)
{

	for (list<tree*>::iterator it = trees.begin(); it != trees.end(); it++)
	{
		if ((**it).root->content == rt)
		{
			if (!(**it).search((**it).root, res, (**it).root))
				return false;
			(**it).delResponse(res);
			return true;
		}
	}
	return false;
}

void treeList::printTree(string rt)
{
	for (list<tree*>::iterator it = trees.begin(); it != trees.end(); it++)
	{
		if ((**it).root->content == rt)
		{
			(**it).printTree();
			return;
		}
	}
	return;
}


void treeList::printAllTrees() 
{
	for (list<tree*>::iterator it = trees.begin(); it != trees.end(); it++)
		(**it).printTree();
	return;
}
void treeList::printSubTree(string rt, string s)
{
	for (list<tree*>::iterator it = trees.begin(); it != trees.end(); it++)
	{
		if ((**it).root->content == rt)
		{
			Node* temp = (**it).search((**it).root, s, (**it).root);
			if (temp)
			{
				(**it).printTree(temp, 0);
				return;
			}
		}
	}
}



