import _thread
import urequests
import json  # Cambiar ujson por json
import time
from MHZ14A import CO2, calibrate_zero_point, calibrate_span_point
from BMP280 import ATM
from MPX5010 import PRESION

CO2_acumulada  = []
Pdif_acumulada = []
P_acumulada    = []

O2_acumulada = [20.95]

# Configura el pin del ESP32 que va al relé (bomba)
#relay_pin = Pin(5, Pin.OUT)  # Asegúrate de conectar la bomba al pin adecuado

#def control_bomba():
#    while True:
#        print("Activando bomba por 1 minuto")
#        relay_pin.value(1)  # Enciende el relé (bomba)
 #       time.sleep(60)      # Mantiene la bomba encendida por 60 segundos (1 minuto)
        
  #      print("Apagando bomba por 30 segundos")
   #     relay_pin.value(0)  # Apaga el relé (bomba)
    #    time.sleep(30)      # Mantiene la bomba apagada por 30 segundos
        
def send_data(data):
    url = 'http://192.168.137.1:5000'
    headers = {'Content-Type': 'application/json'}

    try:
        response = urequests.post(url, json=data, headers=headers, timeout=10)  # Aumenta el tiempo de espera a 10 segundos
        response.close()
        print('Datos enviados con éxito')
    except Exception as e:
        print('Error al realizar la petición:', e)

def inicial_Calibration():
    #Calibración opcional
    calibrate_zero_point()
    calibrate_span_point()

def start_PRESION():
    # Esta función debe devolver el valor de presión
    pressure_mbar = PRESION()
    print("Pressure: {:.3f} mbar".format(pressure_mbar))
    return pressure_mbar

def start_CO2():
    # Esta función debe devolver la concentración de CO2
    co2_concentration = CO2(calibrate=False)  # No calibrar aquí
    print("CO2 Concentration: {:.2f} %".format(co2_concentration))
    return co2_concentration

def start_ATM():
    # Esta función debe devolver la presión atmosférica y temperatura calibrada
    temperature_calibrated, pressure_calibrated = ATM()
    print("Temperature = {:.2f} ºC".format(temperature_calibrated))
    print("Pressure = {:.2f} mbar".format(pressure_calibrated))
    return temperature_calibrated, pressure_calibrated

def read_data(i):
    t1 = i-2
    t2 = i-1
    
    # Leer los datos de cada sensor
    co2_value = start_CO2()
    presion_value = start_PRESION()
    temperature_value, atm_value = start_ATM()

    # Inicializar O2_value
    O2_value = O2_acumulada[-1] if O2_acumulada else 20.95  # Usa el último valor de O2 acumulado o un valor inicial por defecto
    
    CO2_acumulada.append(co2_value)
    Pdif_acumulada.append(presion_value)
    P_acumulada.append(atm_value)

    if i != 0:
        #print(f'\n\ni : {i}, t1 : {t1}, t2 : {t2}')
        #print(f'\n\n====== {O2_acumulada} =====')
        #print(f'====== {CO2_acumulada} =====')
        #print(f'====== {Pdif_acumulada} =====')
        #print(f'====== {P_acumulada} =====\n\n')

        # Cálculo de O2
        O2_value = O2_acumulada[t1] - (CO2_acumulada[t2] - CO2_acumulada[t1]) + (Pdif_acumulada[t2] - Pdif_acumulada[t1]) / (P_acumulada[t2] + Pdif_acumulada[t2]) * 100
        O2_acumulada.append(O2_value)
        print(O2_acumulada)

    print("O2 Value ---> {:.2f}".format(O2_value))
    
    # Verifica si todos los datos han sido obtenidos
    if co2_value is not None and presion_value is not None and atm_value is not None:
        # Datos que deseas enviar
        datos = {
            'PresionAtm': atm_value,
            'PresionDif': presion_value,
            'Co2': co2_value,
            'Temperatura': temperature_value,
            'O2': O2_value  # Añadir O2 a los datos
        }

        # Convertir los datos a JSON
        datos_json = json.dumps(datos)

        # URL del API a la que quieres enviar los datos
        url = "http://192.168.137.1:5000/insert"  # Cambia la URL si es necesario

        # Realizar la petición POST
        try:
            response = urequests.post(url, data=datos_json, headers={"Content-Type": "application/json"})
            print("Código de respuesta:", response.status_code)
            print("Respuesta del servidor:", response.text)
            response.close()
        except Exception as e:
            print("Error al realizar la petición:", e)
    else:
        print("No se obtuvieron todos los datos de los sensores.")


def main():
    print('\nIniciando Calibración... Por favor esperar 20min\n')
    inicial_Calibration()
    
    i = 0
    # Mantén el script principal corriendo en un bucle infinito
    while True:
        time.sleep(5)  # Pausa por 5 segundos antes de ejecutar la siguiente lectura/envío
        try:
            # Inicia la lectura de datos en un nuevo hilo cada 5 segundos
            _thread.start_new_thread(lambda: read_data(i), ())
            i += 1
        except Exception as e:
            print('Error al iniciar el hilo:', e)
       
       
# Esto asegura que el código solo se ejecute si este archivo se ejecuta directamente
if __name__ == '__main__':
    main()