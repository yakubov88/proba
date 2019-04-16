
public function uploadFile($id, $departmant)
    {



        if (!empty($_FILES['files']['name'])) {

            if (!is_dir('uploads/' . $id . '/' . $departmant)) {
                mkdir('./uploads/' . $id . '/' . $departmant, 0777, TRUE);
            }
            $filesCount = count($_FILES['files']['name']);
            for ($i = 0; $i < $filesCount; $i++) {
                $_FILES['file']['name'] = $_FILES['files']['name'][$i];
                $_FILES['file']['type'] = $_FILES['files']['type'][$i];
                $_FILES['file']['tmp_name'] = $_FILES['files']['tmp_name'][$i];
                $_FILES['file']['error'] = $_FILES['files']['error'][$i];
                $_FILES['file']['size'] = $_FILES['files']['size'][$i];

                // File upload configuration
                $uploadPath = 'uploads/' . $id . '/' . $departmant;
                $config['upload_path'] = $uploadPath;
                $config['allowed_types'] = '*';

                // Load and initialize upload library
                $this->load->library('upload', $config);
                $this->upload->initialize($config);

                // Upload file to server
                if ($this->upload->do_upload('file')) {
                    // Uploaded file data
                    $fileData = $this->upload->data();
                    $uploadData[$i]['file_name'] = $fileData['file_name'];
                    $uploadData[$i]['uploaded_on'] = date("Y-m-d H:i:s");
                    $data = array(
                        'claim_name' => $id,
                        'file_name' => $fileData['file_name'],
                        'department' => $departmant

                    );
                    $this->claimmodel->addFile($data);
                }
            }
        }
    }
                
                
    public function addFile($data = array())
    {
        return $this->db->insert('files', $data);
    }            
