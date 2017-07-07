import httplib

class Client:
    def __init__(self):
        self.server = "localhost"
        self.port = 4000
    def request(self, resource):
        conn = httplib.HTTPConnection(self.server + ":" + str(self.port))
        conn.request("GET", resource) # the GET has side effects, we use RMM 1

import sys

def create_player(id, x, y):
    print "Creating player:" + str(id) + " " + str(x) + " " + str(y)
    client = Client()
    client.request("/create/S" + str(id) + "S" + str(x) + "S" + str(y))

def delete_player(id):
    print "Deleting player:" + str(id)
    client = Client()
    client.request("/delete/S" + str(id))

def usage():
    print "To use:"
    print " - create player: python client.py --create <playerid> <x> <y>"
    print " - delete player: python client.py --delete <playerid>"

arglen = len(sys.argv)
if arglen >= 2:
    command = sys.argv[1]
    if command == "--create":
        if arglen != 5:
            print "Arguments are wrong!"
            usage()
        else:
            create_player(sys.argv[2], sys.argv[3], sys.argv[4])
    elif command == "--delete":
        if arglen != 3:
            print "Arguments are wrong!"
            usage()
        else:
            delete_player(sys.argv[2])
    else:
        print "Arguments are wrong!"
        usage()
else:
    usage()
