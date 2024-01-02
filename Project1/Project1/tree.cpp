#pragma once
#include "tree.h"
#include <iostream>
#include <list>
#include <string>
#include "Node.h"
using namespace std;

//A shell to destroy to delete the sons	
void tree::deleteAllSubTree(Node* t)
{
	//if it is empty
	if (t == NULL)
		return;
	//if it a leaf
	if (t->isLeaf)
		delete t;
	//go through all of the sons
	for (auto it = t->responses.begin(); it != t->responses.end(); t++)
		deleteAllSubTree(*it);
}

//Building a root for a tree with a certain value
void tree::addNewTree(string newContent)
{
//	deleteAllSubTree(this->root);
	Node* temp = new Node(newContent);
	root = temp;
}

//Getting a string and returning a pointer to the Node where this string is.
Node* tree::search(Node* p, string val, Node*& parent)
{
	if (p == nullptr) return nullptr;//no tree, no val
	if (p->content == val) return p;//found, return Node*
	if (p->isLeaf) return nullptr;//leaf, didn't find val
	//for (auto it = p->responses.begin();it != p->responses.end();it++)
	for (list<Node*>::iterator it = p->responses.begin(); it != p->responses.end(); it++)
	{
		Node* ezer = search(*it, val, p);
		if (ezer != nullptr)//found
			return ezer;
	}
	return nullptr;//didn't find

}

// Adding a vertex to the tree, as a child of another vertex, given 2 strings, a string of the father already in the tree and a string that should be in the new son
void tree::addResponse(string fatherdiscussion, string newresponse)
{
	Node* help = search(root,fatherdiscussion,root);
	//if this is a leaf
	if (help->isLeaf)
		help->isLeaf = false;
	Node *temp=new Node(newresponse);
	help->responses.push_back(temp);
}

void tree::delResponse(string discussion)
{
	if (root == nullptr)
		return;
	Node* temp = search(root,discussion,root);
	deleteAllSubTree(temp);
	Node* father = findFather(root, discussion, root);
	temp = nullptr;
	if (father->responses.empty())
		father->isLeaf = true;
	return;
}

Node* tree::findFather(Node* p, string val, Node*& parent)
{
	if (p == nullptr) return nullptr;//no tree, no val
	if (p->content == val) return parent;//found, return Node*
	//if (p->isLeaf) return nullptr;//leaf, didn't find val
	//for (auto it = p->responses.begin();it != p->responses.end();it++)
	for (list<Node*>::iterator it = p->responses.begin(); it != p->responses.end(); it++)
	{
		Node* ezer = findFather(*it, val, p);
		if (ezer != nullptr)//found
			return ezer;
	}
	return nullptr;//didn't find
}
//
void tree::printTree()
{
	printTree(root,0);
}

void tree::printTree(Node* p,int num)
{
	if (p == nullptr)
		return;
	for (int i = 0; i < num; i++)
		cout << " ";
	cout << p->content << endl;
	for (list<Node*>::iterator it = p->responses.begin(); it != p->responses.end(); it++)
	{
		printTree(*it,num+3);
	}
	return;
}

//Searching for a vertex with this string, and printing the path from this vertex to the root.
void tree::searchPrint(string discussion)
{
	cout << discussion << endl;
	Node* temp = findFather(root, discussion, root);
	while (temp != root)
	{
		cout << temp->content << endl;
		temp=findFather(temp, discussion, temp);
	}
	cout << temp->content;
	return;
}
//That is, printing all discussion branched starting from this string.
void tree::printPart(string discussion)
{
	Node* temp = search(root, discussion, root);
	printTree(temp, 0);
	return;
}

//void tree::printPath(Node* temp,string s)
//{
//	if (temp->content == s)
//		return;
//	for (list<Node*>::iterator it = temp->responses.begin(); it != temp->responses.end(); it++)
//	{
//		if()
//	}
//}