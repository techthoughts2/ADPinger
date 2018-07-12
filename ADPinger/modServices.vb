Imports System.ServiceProcess
Imports System.Threading

Module modServices
    'this module doesn't contain a class persay and instead for readability contains the various
    'functions required for interacting with services on remote devices

    Function serviceStatus(ByVal sServiceName As String, ByVal actionPerform As Integer, ByVal pcName As String)
        'this function will determine the current status of the service on the remote device.
        'if all we are after is the status then it will simply return the value.
        'if we will be stopping/starting/restarting services this function will determine the first step that will actually
        'be taken.  for example, we will not try to stop a service that is already reporting as stopped.

        Dim strResults As String = "" 'the string that will be returned to the main form
        Dim statusCode As Integer 'service status is orginially returned as a number 1-7

        'here we will encapsulate all tasks inside a using.
        'this ensures that the service controller is properly disposed of 
        Using sc As ServiceController = New ServiceController(sServiceName, pcName)
            Try
                '########################GET SERVICE STATUS###########################################
                'lets try and get the current status of the specified service
                statusCode = sc.Status

                'at this point we have a number, not lets form that into usable information for the user
                If statusCode = 4 Then
                    strResults = "Running"
                ElseIf statusCode = 1 Then
                    strResults = "Stopped"
                ElseIf statusCode = 7 Then
                    strResults = "Paused"
                ElseIf statusCode = 3 Then
                    strResults = "StopPending"
                ElseIf statusCode = 2 Then
                    strResults = "StartPending"
                ElseIf statusCode = 6 Then
                    strResults = "PausePending"
                ElseIf statusCode = 5 Then
                    strResults = "ContinuePending"
                End If
                '########################END GET SERVICE STATUS#######################################

                '########################PERFORM SERVICE ACTIONS######################################
                'the action to perform was passed by the parent form
                If actionPerform = 1 Then
                    'action 1 is to just get scan results. at this point we already have those results
                    'lets just return those results to the parent form
                    Return strResults
                ElseIf actionPerform = 2 Then
                    'this indicates the user wants to restart the service
                    'note that we will be calling both start and stop functions here
                    If sc.Status <> 1 Then
                        'only if the service is currently not set to stop, we will send a stop command
                        strResults = stopService(sc)
                        'lets check to see if we were successful in stopping the service
                        If strResults = "Service Stopped" Then
                            'we will then add to the return string by starting the service
                            strResults = strResults & " THEN: " & startServices(sc)
                            Return strResults
                        Else
                            'we didn't successfully stop the service, so there is no point in trying to start it again
                            strResults = strResults & " - Start service command not sent"
                            Return strResults
                        End If
                    Else
                        'in this circumstance the service is already stopped, we will simply start it again
                        strResults = "Service already Stopped"
                        strResults = strResults & " THEN: " & startServices(sc)
                        Return strResults
                    End If
                ElseIf actionPerform = 3 Then
                    'the user wants to stops the service
                    If sc.Status <> 1 Then
                        'only if the service is not set stop will we send a stop command
                        strResults = stopService(sc)
                        Return strResults
                    Else
                        'advised the user it was already stopped
                        strResults = "Service already Stopped"
                        Return strResults
                    End If
                ElseIf actionPerform = 4 Then
                    'the user wants to start the servce
                    If sc.Status <> 4 Then
                        'only if the service is not se to start will we send a start command
                        strResults = startServices(sc)
                        Return strResults
                    End If
                End If
                '########################END PERFORM SERVICE ACTIONS##################################
            Catch ex As Exception
                'MsgBox(ex.ToString)
                'if we can't get a status on the service the most likely cause is that the service doesn't exist on the device
                If ex.ToString.Contains("service does not exist") Then
                    'so, we will look into the error message itself, if service is missing, let the user know
                    strResults = "Service does not exist"
                    Return strResults
                Else
                    'otherwise, something just went wrong
                    strResults = "Error interacting with remote service"
                    Return strResults
                End If
                'send back the results
                Return strResults
                Exit Function
            End Try
        End Using 'dispose of servicecontroller

    End Function

    Function stopService(ByRef serviceCon)
        'this function will send a stop command to the remote service


        Dim actionStatus As String 'form string for status of what happens with the service
        '##################Begin stop services##############################################
        Try
            'note that serviceCon is passed from the status function so that we are still working with
            'the same service name. lets go ahead and send that stop command now
            serviceCon.Stop()
            Try
                'EXPERIMENTAL. NOT SURE IF I LIKE THIS
                'we will now wait up to 45 seconds for the service to return as stopped.
                'this second figure may need to be adjusted later. this is difficult to test
                serviceCon.WaitForStatus(ServiceControllerStatus.Stopped, New TimeSpan(0, 0, 45))
            Catch ex As Exception
                'this catch is seperately needed because this is where we wind up when the time goes over
                actionStatus = "Stop sent but no change in service status after 45secs"
                Return actionStatus
                Exit Function
            End Try
            'the service successfully stopped
            actionStatus = "Service Stopped"
            Return actionStatus
        Catch ex As Exception
            'something went wrong trying to stop the service
            actionStatus = "Unable to stop service"
            Return actionStatus
        End Try
        '##################END stop services################################################

    End Function
    Function startServices(ByRef serviceCon)
        'this function will send a start command to the remote service

        Dim actionStatus As String 'form string for status of what happens with the service

        '##################Begin start services##############################################
        Try
            'note that serviceCon is passed from the status function so that we are still working with
            'the same service name. lets go ahead and send that start command now
            serviceCon.Start()
            Try
                'EXPERIMENTAL. NOT SURE IF I LIKE THIS
                'we will now wait up to 45 seconds for the service to return as started.
                'this second figure may need to be adjusted later. this is difficult to test
                serviceCon.waitforstatus(ServiceControllerStatus.Running, New TimeSpan(0, 0, 45))
            Catch ex As Exception
                'this catch is seperately needed because this is where we wind up when the time goes over
                actionStatus = "Start sent but no change in service status after 45secs"
                Return actionStatus
                Exit Function
            End Try
            'the service successfully stopped
            actionStatus = "Service Started"
            Return actionStatus
        Catch ex As Exception
            actionStatus = "Unable to start service"
            Return actionStatus
        End Try
        '##################END start services################################################

    End Function
End Module
