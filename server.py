import socket
import re
import sys

from time import sleep
from subprocess import call

port = 0
if len(sys.argv) == 1:
	port = 8000
else:
	port = int(sys.argv[1])
print "Server starting on port " + str(port)

################################################################################

host = ''
sock = socket.socket(socket.AF_INET, socket.SOCK_STREAM)

# allows reuse of the socket once the process gets stopped
sock.setsockopt(socket.SOL_SOCKET, socket.SO_REUSEADDR, 1)

#bind the connection
sock.bind((host, port))

#listen to one client
sock.listen(1)

################################################################################

import signal
import sys
import json

#register a specific signal handler if necessary at the termination of the program
# def signal_handler(signal, frame):
#     # close the socket here
#     sys.exit(0)
# signal.signal(signal.SIGINT, signal_handler)

################################################################################

from player import Player
from objects import Object
from player_controller import PlayerController

from exports import get_players
from exports import update_players
from exports import get_objects
from exports import update_objects

def send_str(csock, message):
    csock.send(message)

def receive_lines(csock):
    # receive the GET from front-ent
    req = csock.recv(1024)

    # # process the received input
    lines = req.split('\n')

    return lines

class JSender():
	def __init__(self, csock):
		self.csock = csock
	def send(self, jresource):
		print "Sending jresource: " + str(jresource)
		sresouce = json.dumps(jresource)
		send_str(self.csock, sresouce)

def serve(csock, resource):
  jsender = JSender(csock)

  controller = PlayerController()

  #listen
  get_request = receive_lines(csock)[0]
  resource_identifier = get_request.split(' ')[1]

  print resource_identifier

  if resource_identifier.find("create") != -1:
    print "Creating new player"
    pack = resource_identifier.split("S")
    player = Player(pack[1], pack[2], pack[3])
    controller.add_player(player)

  if resource_identifier.find("delete") != -1:
    print "Delete player"
    pack = resource_identifier.split("S")
    player_id = pack[1]
    controller.delete_player(player_id)

  if resource_identifier == "/start":
	  jsender.send({
	  	"minX" : -10,
		"minY" : -10,
		"maxX" : 10,
		"maxY" : 10,
	  	"objects": [obj.json() for obj in resource["objects"]],
		"players": [obj.json() for obj in resource["players"]]
	  })

  if resource_identifier == "/update":
    jsender.send({
      "players": [obj.json() for obj in resource["players"]],
	  "new_players": [obj.json() for obj in controller.get_new_players()],
	  "deleted_players": [obj.json() for obj in controller.get_deleted_players()]
    })

  # update the model
  update_players(resource["players"])
  update_objects(resource["objects"])

  # close connection
  csock.close()
  sleep(1)

################################################################################

resource = {
	"players" : get_players(),
	"objects" : get_objects()
}

#listen for connection
while True:
    csock, caddr = sock.accept()
    print "Connection from:" + `caddr`

    # serve the connection
    serve(csock, resource)
