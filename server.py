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

#register a specific signal handler if necessary at the termination of the program
# def signal_handler(signal, frame):
#     # close the socket here
#     sys.exit(0)
# signal.signal(signal.SIGINT, signal_handler)

################################################################################

def send_str(csock, message):
    csock.send(message)

def receive_lines(csock):
    # receive the GET from front-ent
    req = csock.recv(1024)

    # # process the received input
    lines = req.split('\n')

    return lines

def serve(csock):
    receive_lines(csock)
    send_str(csock, "Helo")

    csock.close()
    sleep(1)

#listen for connection
while True:
    csock, caddr = sock.accept()
    print "Connection from:" + `caddr`

    # serve the connection
    serve(csock)
