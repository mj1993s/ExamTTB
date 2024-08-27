import React, { useEffect, useState } from 'react';
import Box from '@mui/material/Box';
import TextField from '@mui/material/TextField';
import Typography from '@mui/material/Typography';
import Button from '@mui/material/Button';
import AddIcon from '@mui/icons-material/Add';
import EditIcon from '@mui/icons-material/Edit';
import DeleteIcon from '@mui/icons-material/DeleteOutlined';
import SaveIcon from '@mui/icons-material/Save';
import CancelIcon from '@mui/icons-material/Close';

import {
  GridRowModes,
  DataGrid,
  GridToolbarContainer,
  GridActionsCellItem,
  GridRowEditStopReasons,
} from '@mui/x-data-grid';


// function EditToolbar(props) {
//   const { setRows, setRowModesModel } = props;

//   const handleClick = () => {
//     const id = randomId();
//     setRows((oldRows) => [...oldRows, { id, name: '', age: '', isNew: true }]);
//     setRowModesModel((oldModel) => ({
//       ...oldModel,
//       [id]: { mode: GridRowModes.Edit, fieldToFocus: 'name' },
//     }));
//   };

//   return (
//     <GridToolbarContainer>
//       <Button color="primary" startIcon={<AddIcon />} onClick={handleClick}>
//         Add record
//       </Button>
//     </GridToolbarContainer>
//   );
// }

export default function ShopGrid() {
  const [rows, setRows] = useState([]);
  const [rowModesModel, setRowModesModel] = React.useState({});
  const [ColVisible, setColVisible] = useState({ cartId: false, stockId: false });
  const [sum, setSum] = useState({});

  useEffect(() => {
    CartGet()
  }, [])

  const CartGet = () => {
    fetch("http://localhost:5000/GetCartList")
      .then(res => res.json())
      .then(
        (result) => {
          setSum({ Total: result.reduce((a, v) => a = a + v.cartTotalPrice, 0) })
          setRows(result);
        }
      )
      .catch(rejected => {
        alert(rejected);
      })
  }

  const CartUpdate = (cart) => {
    fetch('http://localhost:5000/UpdateCart?cartId=' + cart.cartId, {
      method: 'PUT',
      headers: {
        Accept: 'application/form-data',
        'Content-Type': 'application/json',
      },
      body: JSON.stringify({ stockId: cart.stockId, qty: cart.cartQty }),
    })
      .then(res => res.json())
      .then(
        (result) => {
          alert(result['message'])
          CartGet()
        }
      )
      .catch(rejected => {
        alert(rejected);
      })
  }

  const CartDelete = (cartId) => {
    fetch('http://localhost:5000/DeleteCart?cartId=' + cartId, {
      method: 'DELETE',
      headers: {
        Accept: 'application/form-data',
        'Content-Type': 'application/json',
      }
    })
      .then(res => res.json())
      .then(
        (result) => {
          alert(result['message'])
          CartGet()
        }
      )
      .catch(rejected => {
        alert(rejected);
      })
  }


  const handleRowEditStop = (params, event) => {
    if (params.reason === GridRowEditStopReasons.rowFocusOut) {
      event.defaultMuiPrevented = true;
    }
  };

  const handleEditClick = (cartId) => () => {
    setRowModesModel({ ...rowModesModel, [cartId]: { mode: GridRowModes.Edit } });
  };

  const handleSaveClick = (cartId) => () => {
    setRowModesModel({ ...rowModesModel, [cartId]: { mode: GridRowModes.View } });
  };

  const handleDeleteClick = (id) => () => {
    CartDelete(id)
    setRows(rows.filter((row) => row.cartId !== id));
  };

  const handleCancelClick = (cartId) => () => {
    setRowModesModel({
      ...rowModesModel,
      [cartId]: { mode: GridRowModes.View, ignoreModifications: true },
    });

    const editedRow = rows.find((row) => row.cartId === cartId);
    if (editedRow.isNew) {
      setRows(rows.filter((row) => row.cartId !== cartId));
    }
  };

  const processRowUpdate = (newRow) => {
    CartUpdate(newRow)

    const updatedRow = { ...newRow, isNew: false };
    setRows(rows.map((row) => (row.cartId === newRow.cartId ? updatedRow : row)));
    return updatedRow;
  };

  const handleRowModesModelChange = (newRowModesModel) => {
    setRowModesModel(newRowModesModel);
  };

  const columns = [
    {
      field: 'No', headerName: '#', flex: 0, editable: false,
      renderCell: (params) => params.api.getAllRowIds().indexOf(params.row.cartId) + 1
    },
    {
      field: 'cartId',
      editable: false,
    },
    {
      field: 'stockId',
      editable: false,
    },
    {
      field: 'prodName',
      headerName: 'Product Name',
      width: 200,
      editable: false,
    },
    {
      field: 'cartQty',
      headerName: 'Qty',
      width: 80,
      editable: true,
    },
    {
      field: 'cartTotalPrice',
      headerName: 'Total Price',
      width: 100,
      type: 'number',
      editable: false,
    },
    {
      field: 'actions',
      type: 'actions',
      headerName: 'Actions',
      width: 100,
      cellClassName: 'actions',
      getActions: ({ id }) => {
        const isInEditMode = rowModesModel[id]?.mode === GridRowModes.Edit;

        if (isInEditMode) {
          return [
            <GridActionsCellItem
              icon={<SaveIcon />}
              label="Save"
              sx={{
                color: 'primary.main',
              }}
              onClick={handleSaveClick(id)}
            />,
            <GridActionsCellItem
              icon={<CancelIcon />}
              label="Cancel"
              className="textPrimary"
              onClick={handleCancelClick(id)}
              color="inherit"
            />,
          ];
        }

        return [
          <GridActionsCellItem
            icon={<EditIcon />}
            label="Edit"
            className="textPrimary"
            onClick={handleEditClick(id)}
            color="inherit"
          />,
          <GridActionsCellItem
            icon={<DeleteIcon />}
            label="Delete"
            onClick={handleDeleteClick(id)}
            color="inherit"
          />,
        ];
      },
    },
  ];

  const [stocks, setStocks] = useState([]);

  useEffect(() => {
    StockGet()
  }, [])

  const CartCreate = (cart) => {
    fetch('http://localhost:5000/CreateCart', {
      method: 'POST',
      headers: {
        Accept: 'application/form-data',
        'Content-Type': 'application/json',
      },
      body: JSON.stringify({ stockId: cart.stockId, Qty: 1 }),
    })
      .then(res => res.json())
      .then(
        (result) => {
          alert(result['message'])
          CartGet()
        }
      )
      .catch(rejected => {
        alert(rejected);
      })
  }

  const StockGet = () => {
    fetch("http://localhost:5000/GetStockList")
      .then(res => res.json())
      .then(
        (result) => {
          //console.log(result);
          let stockList = [];
          for (let i = 0; i < result.length; i++) {
            let dataStock = result[i];

            stockList.push(<Box
              height={80}
              width={200}
              my={4}
              display="flex"
              alignItems="center"
              gap={4}
              p={2}
              sx={{ border: '2px solid grey' }}>
              {dataStock.prodName} {dataStock.prodPrice} บาท
              <br></br>
              เหลือ {dataStock.stockQty}
              <br></br>
              <Button variant="contained" onClick={() => CartCreate(dataStock)}>Add</Button>
            </Box>);
          }


          setStocks(stockList)
        }
      )
      .catch(rejected => {
        alert(rejected);
      })
  }


  const CheckOut = () => {
    fetch('http://localhost:5000/CheckOutCart', {
      method: 'POST',
      headers: {
        Accept: 'application/form-data',
        'Content-Type': 'application/json',
      }
    })
      .then(res => res.json())
      .then(
        (result) => {
          alert(result['message'])
          CartGet()
          StockGet()
        }
      )
      .catch(rejected => {
        alert(rejected);
      })
  }

  const styleCheckOut = {
    'padding-bottom': '22px'
  }

  const style = {
    'padding-left': '20%',
    'padding-right': '20%'
  }

  return (
    <div style={style}>
      <div>{stocks}</div>
      <div style={styleCheckOut}>
        ราคาทั้งหมด: {sum.Total} บาท
        <br></br>
        <Button variant="contained" onClick={() => CheckOut()} > Check Out</Button>
      </div>
      <div> 
        <Box
          sx={{
            height: 300,
            width: '100%',
            '& .actions': {
              color: 'text.secondary',
            },
            '& .textPrimary': {
              color: 'text.primary',
            },
          }}
        >
          <DataGrid
            rows={rows}
            columns={columns}
            columnVisibilityModel={ColVisible}
            getRowId={(row) => row.cartId}
            editMode="row"
            rowModesModel={rowModesModel}
            onRowModesModelChange={handleRowModesModelChange}
            onRowEditStop={handleRowEditStop}
            processRowUpdate={processRowUpdate}
            slotProps={{
              toolbar: { setRows, setRowModesModel },
            }}
          />
        </Box>
      </div>
    </div>
  );
}