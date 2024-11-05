import network
import time
import main

def connect_to_wifi():
    ssid = 'VALEN-PC'
    password = 'shirayukixzen'
    
    # Inicializa la interfaz Wi-Fi
    station = network.WLAN(network.STA_IF)
    station.active(True)
    
    if station.isconnected():
        print('Ya está conectado a la red Wi-Fi')
        return station

    # Conecta a la red Wi-Fi
    station.connect(ssid, password)
    print('Intentando conectar a la red Wi-Fi...')
    
    # Espera hasta que se conecte
    start_time = time.time()
    timeout = 20  # tiempo de espera en segundos
    
    while not station.isconnected():
        if time.time() - start_time > timeout:
            print('Tiempo de espera agotado. No se pudo conectar a la red Wi-Fi.')
            return None
        print('Conectando a la red Wi-Fi...')
        time.sleep(1)
    
    print('Conectado a la red Wi-Fi')
    print('Detalles de la conexión:', station.ifconfig())
    return station

# Llama a la función de conexión Wi-Fi
wifi_station = connect_to_wifi()

# Verifica si la conexión fue exitosa antes de continuar
if wifi_station and wifi_station.isconnected():
    # Si está conectado, inicia la lectura de datos en el main.py
    main.main()
else:
    print('No se pudo establecer la conexión inicial a la red Wi-Fi.')
