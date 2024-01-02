#pragma once
#include <iostream>
#include <list>
#include <string>
#include "Node.h"
using namespace std;
class tree
{
	Node* root;
public:
	//con
	tree() { root = NULL; }
	//des
	~tree() {
		deleteAllSubTree(root);
		root = 0;
	}
	//A shell to destroy to delete the sons
	void deleteAllSubTree(Node* t);
	// Building a root for a tree with a certain value
	void addNewTree(string newContent);
	//Getting a string and returning a pointer to the Node where this string is.
	Node* search(Node* p, string val, Node*& parent);
	// Adding a vertex to the tree, as a child of another vertex, given 2 strings, a string of the father already in the tree and a string that should be in the new son
	void addResponse(string fatherdiscussion, string newresponse);
	//Deleting the entire subtree of the string, including the string itself
	void delResponse(string discussion);
	//print tree
	void printTree();
	//Searching for a vertex with this string, and printing the path from this vertex to the root.
	void searchPrint(string discussion);
	// Given a certain value, searching for a vertex with this string, and printing the entire subtree from the text of that string to all the leaves, in a hierarchical form (with 3 spaces per page level).
	//That is, printing all discussion branched starting from this string.
	void printPart(string discussion);
	//
	Node* findFather(Node* p, string val, Node*& parent);
	void printTree(Node* p,int num);
	friend class treeList;
	void printPath(Node* temp,string s);

};

