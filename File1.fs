                using (Transaction trans = new Transaction(doc))
                {
                    try
                    {
                        trans.Start("Crear tipos rebartype");
  
                        trans.Commit();
                    }
                    catch (Exception ex)
                    {
                        string message = ex.Message;
                        TaskDialog.Show("Error", message);
                        return;
                    }
                }